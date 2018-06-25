using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	public class JobDriver_DoBill : JobDriver
	{
		public float workLeft;

		public int billStartTick;

		public int ticksSpentDoingRecipeWork;

		public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

		public const TargetIndex BillGiverInd = TargetIndex.A;

		public const TargetIndex IngredientInd = TargetIndex.B;

		public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;

		public JobDriver_DoBill()
		{
		}

		public override string GetReport()
		{
			string result;
			if (this.job.RecipeDef != null)
			{
				result = base.ReportStringProcessed(this.job.RecipeDef.jobString);
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		public IBillGiver BillGiver
		{
			get
			{
				IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver == null)
				{
					throw new InvalidOperationException("DoBill on non-Billgiver.");
				}
				return billGiver;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.billStartTick, "billStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksSpentDoingRecipeWork, "ticksSpentDoingRecipeWork", 0, false);
		}

		public override bool TryMakePreToilReservations()
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddEndCondition(delegate
			{
				Thing thing = base.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
				JobCondition result;
				if (thing is Building && !thing.Spawned)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			});
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn(delegate()
			{
				IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver != null)
				{
					if (this.job.bill.DeletedOrDereferenced)
					{
						return true;
					}
					if (!billGiver.CurrentlyUsableForBills())
					{
						return true;
					}
				}
				return false;
			});
			Toil gotoBillGiver = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.job.targetQueueB != null && this.job.targetQueueB.Count == 1)
					{
						UnfinishedThing unfinishedThing = this.job.targetQueueB[0].Thing as UnfinishedThing;
						if (unfinishedThing != null)
						{
							unfinishedThing.BoundBill = (Bill_ProductionWithUft)this.job.bill;
						}
					}
				}
			};
			yield return Toils_Jump.JumpIf(gotoBillGiver, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return extract;
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return getToHaulTarget;
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, true, false, true);
			yield return JobDriver_DoBill.JumpToCollectNextIntoHandsForBill(getToHaulTarget, TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDestroyedOrNull(TargetIndex.B);
			Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
			yield return findPlaceTarget;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false);
			yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, extract);
			yield return gotoBillGiver;
			yield return Toils_Recipe.MakeUnfinishedThingIfNeeded();
			yield return Toils_Recipe.DoRecipeWork().FailOnDespawnedNullOrForbiddenPlacedThings().FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_Recipe.FinishRecipeAndStartStoringProduct();
			if (!this.job.RecipeDef.products.NullOrEmpty<ThingDefCountClass>() || !this.job.RecipeDef.specialProducts.NullOrEmpty<SpecialProductType>())
			{
				yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
				Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				yield return carryToCell;
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
				Toil recount = new Toil();
				recount.initAction = delegate()
				{
					Bill_Production bill_Production = recount.actor.jobs.curJob.bill as Bill_Production;
					if (bill_Production != null && bill_Production.repeatMode == BillRepeatModeDefOf.TargetCount)
					{
						this.Map.resourceCounter.UpdateResourceCounts();
					}
				};
				yield return recount;
			}
			yield break;
		}

		private static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error("JumpToAlsoCollectTargetInQueue run on " + actor + " who is not carrying something.", false);
				}
				else if (!actor.carryTracker.Full)
				{
					Job curJob = actor.jobs.curJob;
					List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
					if (!targetQueue.NullOrEmpty<LocalTargetInfo>())
					{
						for (int i = 0; i < targetQueue.Count; i++)
						{
							if (GenAI.CanUseItemForWork(actor, targetQueue[i].Thing))
							{
								if (targetQueue[i].Thing.CanStackWith(actor.carryTracker.CarriedThing))
								{
									if ((float)(actor.Position - targetQueue[i].Thing.Position).LengthHorizontalSquared <= 64f)
									{
										int num = (actor.carryTracker.CarriedThing != null) ? actor.carryTracker.CarriedThing.stackCount : 0;
										int num2 = curJob.countQueue[i];
										num2 = Mathf.Min(num2, targetQueue[i].Thing.def.stackLimit - num);
										num2 = Mathf.Min(num2, actor.carryTracker.AvailableStackSpace(targetQueue[i].Thing.def));
										if (num2 > 0)
										{
											curJob.count = num2;
											curJob.SetTarget(ind, targetQueue[i].Thing);
											List<int> countQueue;
											int index;
											(countQueue = curJob.countQueue)[index = i] = countQueue[index] - num2;
											if (curJob.countQueue[i] <= 0)
											{
												curJob.countQueue.RemoveAt(i);
												targetQueue.RemoveAt(i);
											}
											actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
											break;
										}
									}
								}
							}
						}
					}
				}
			};
			return toil;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <gotoBillGiver>__0;

			internal Toil <bind>__0;

			internal Toil <extract>__1;

			internal Toil <getToHaulTarget>__1;

			internal Toil <findPlaceTarget>__1;

			internal Toil <carryToCell>__2;

			internal JobDriver_DoBill $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_DoBill.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
				{
					base.AddEndCondition(delegate
					{
						Thing thing = base.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
						JobCondition result;
						if (thing is Building && !thing.Spawned)
						{
							result = JobCondition.Incompletable;
						}
						else
						{
							result = JobCondition.Ongoing;
						}
						return result;
					});
					this.FailOnBurningImmobile(TargetIndex.A);
					this.FailOn(delegate()
					{
						IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
						if (billGiver != null)
						{
							if (this.job.bill.DeletedOrDereferenced)
							{
								return true;
							}
							if (!billGiver.CurrentlyUsableForBills())
							{
								return true;
							}
						}
						return false;
					});
					gotoBillGiver = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
					Toil bind = new Toil();
					bind.initAction = delegate()
					{
						if (this.job.targetQueueB != null && this.job.targetQueueB.Count == 1)
						{
							UnfinishedThing unfinishedThing = this.job.targetQueueB[0].Thing as UnfinishedThing;
							if (unfinishedThing != null)
							{
								unfinishedThing.BoundBill = (Bill_ProductionWithUft)this.job.bill;
							}
						}
					};
					this.$current = bind;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
					this.$current = Toils_Jump.JumpIf(gotoBillGiver, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					extract = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
					this.$current = extract;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
					this.$current = getToHaulTarget;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.B, true, false, true);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = JobDriver_DoBill.JumpToCollectNextIntoHandsForBill(getToHaulTarget, TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDestroyedOrNull(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
					this.$current = findPlaceTarget;
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false);
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					this.$current = Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, extract);
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				case 10u:
					this.$current = gotoBillGiver;
					if (!this.$disposing)
					{
						this.$PC = 11;
					}
					return true;
				case 11u:
					this.$current = Toils_Recipe.MakeUnfinishedThingIfNeeded();
					if (!this.$disposing)
					{
						this.$PC = 12;
					}
					return true;
				case 12u:
					this.$current = Toils_Recipe.DoRecipeWork().FailOnDespawnedNullOrForbiddenPlacedThings().FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
					if (!this.$disposing)
					{
						this.$PC = 13;
					}
					return true;
				case 13u:
					this.$current = Toils_Recipe.FinishRecipeAndStartStoringProduct();
					if (!this.$disposing)
					{
						this.$PC = 14;
					}
					return true;
				case 14u:
					if (!this.job.RecipeDef.products.NullOrEmpty<ThingDefCountClass>() || !this.job.RecipeDef.specialProducts.NullOrEmpty<SpecialProductType>())
					{
						this.$current = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
						if (!this.$disposing)
						{
							this.$PC = 15;
						}
						return true;
					}
					break;
				case 15u:
					carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
					this.$current = carryToCell;
					if (!this.$disposing)
					{
						this.$PC = 16;
					}
					return true;
				case 16u:
					this.$current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
					if (!this.$disposing)
					{
						this.$PC = 17;
					}
					return true;
				case 17u:
					<MakeNewToils>c__AnonStorey.recount = new Toil();
					<MakeNewToils>c__AnonStorey.recount.initAction = delegate()
					{
						Bill_Production bill_Production = <MakeNewToils>c__AnonStorey.recount.actor.jobs.curJob.bill as Bill_Production;
						if (bill_Production != null && bill_Production.repeatMode == BillRepeatModeDefOf.TargetCount)
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map.resourceCounter.UpdateResourceCounts();
						}
					};
					this.$current = <MakeNewToils>c__AnonStorey.recount;
					if (!this.$disposing)
					{
						this.$PC = 18;
					}
					return true;
				case 18u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			Toil IEnumerator<Toil>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_DoBill.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_DoBill.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal JobCondition <>m__0()
			{
				Thing thing = base.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
				JobCondition result;
				if (thing is Building && !thing.Spawned)
				{
					result = JobCondition.Incompletable;
				}
				else
				{
					result = JobCondition.Ongoing;
				}
				return result;
			}

			internal bool <>m__1()
			{
				IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver != null)
				{
					if (this.job.bill.DeletedOrDereferenced)
					{
						return true;
					}
					if (!billGiver.CurrentlyUsableForBills())
					{
						return true;
					}
				}
				return false;
			}

			internal void <>m__2()
			{
				if (this.job.targetQueueB != null && this.job.targetQueueB.Count == 1)
				{
					UnfinishedThing unfinishedThing = this.job.targetQueueB[0].Thing as UnfinishedThing;
					if (unfinishedThing != null)
					{
						unfinishedThing.BoundBill = (Bill_ProductionWithUft)this.job.bill;
					}
				}
			}

			internal bool <>m__3()
			{
				return this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>();
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil recount;

				internal JobDriver_DoBill.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Bill_Production bill_Production = this.recount.actor.jobs.curJob.bill as Bill_Production;
					if (bill_Production != null && bill_Production.repeatMode == BillRepeatModeDefOf.TargetCount)
					{
						this.<>f__ref$0.$this.Map.resourceCounter.UpdateResourceCounts();
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <JumpToCollectNextIntoHandsForBill>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal Toil gotoGetTargetToil;

			public <JumpToCollectNextIntoHandsForBill>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error("JumpToAlsoCollectTargetInQueue run on " + actor + " who is not carrying something.", false);
				}
				else if (!actor.carryTracker.Full)
				{
					Job curJob = actor.jobs.curJob;
					List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(this.ind);
					if (!targetQueue.NullOrEmpty<LocalTargetInfo>())
					{
						for (int i = 0; i < targetQueue.Count; i++)
						{
							if (GenAI.CanUseItemForWork(actor, targetQueue[i].Thing))
							{
								if (targetQueue[i].Thing.CanStackWith(actor.carryTracker.CarriedThing))
								{
									if ((float)(actor.Position - targetQueue[i].Thing.Position).LengthHorizontalSquared <= 64f)
									{
										int num = (actor.carryTracker.CarriedThing != null) ? actor.carryTracker.CarriedThing.stackCount : 0;
										int num2 = curJob.countQueue[i];
										num2 = Mathf.Min(num2, targetQueue[i].Thing.def.stackLimit - num);
										num2 = Mathf.Min(num2, actor.carryTracker.AvailableStackSpace(targetQueue[i].Thing.def));
										if (num2 > 0)
										{
											curJob.count = num2;
											curJob.SetTarget(this.ind, targetQueue[i].Thing);
											List<int> countQueue;
											int index;
											(countQueue = curJob.countQueue)[index = i] = countQueue[index] - num2;
											if (curJob.countQueue[i] <= 0)
											{
												curJob.countQueue.RemoveAt(i);
												targetQueue.RemoveAt(i);
											}
											actor.jobs.curDriver.JumpToToil(this.gotoGetTargetToil);
											break;
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
