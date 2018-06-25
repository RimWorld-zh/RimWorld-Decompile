using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_FoodDeliver : JobDriver
	{
		private bool usingNutrientPasteDispenser;

		private bool eatingFromInventory;

		private const TargetIndex FoodSourceInd = TargetIndex.A;

		private const TargetIndex DelivereeInd = TargetIndex.B;

		public JobDriver_FoodDeliver()
		{
		}

		private Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		public override string GetReport()
		{
			string result;
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				result = this.job.def.reportString.Replace("TargetA", ThingDefOf.MealNutrientPaste.label).Replace("TargetB", this.Deliveree.LabelShort);
			}
			else
			{
				result = base.GetReport();
			}
			return result;
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (base.TargetThingA is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA));
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.B);
			if (this.eatingFromInventory)
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else if (this.usingNutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			}
			else
			{
				yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			Toil toil2 = new Toil();
			toil2.initAction = delegate()
			{
				Pawn actor = toil2.actor;
				Job curJob = actor.jobs.curJob;
				actor.pather.StartPath(curJob.targetC, PathEndMode.OnCell);
			};
			toil2.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil2.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			toil2.AddFailCondition(delegate
			{
				Pawn pawn = (Pawn)toil2.actor.jobs.curJob.targetB.Thing;
				return !pawn.IsPrisonerOfColony || !pawn.guest.CanBeBroughtFood;
			});
			yield return toil2;
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Thing thing;
				this.pawn.carryTracker.TryDropCarriedThing(toil.actor.jobs.curJob.targetC.Cell, ThingPlaceMode.Direct, out thing, null);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_FoodDeliver $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_FoodDeliver.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			private JobDriver_FoodDeliver.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey2 $locvar1;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				Toil toil;
				switch (num)
				{
				case 0u:
					this.FailOnDespawnedOrNull(TargetIndex.B);
					if (this.eatingFromInventory)
					{
						this.$current = Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					if (this.usingNutrientPasteDispenser)
					{
						this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					this.$current = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 1u:
					break;
				case 2u:
					this.$current = Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					break;
				case 4u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Ingest.PickupIngestible(TargetIndex.A, base.Deliveree);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					break;
				case 7u:
				{
					Toil toil = new Toil();
					toil.initAction = delegate()
					{
						Thing thing;
						this.pawn.carryTracker.TryDropCarriedThing(toil.actor.jobs.curJob.targetC.Cell, ThingPlaceMode.Direct, out thing, null);
					};
					toil.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = toil;
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				}
				case 8u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				toil = new Toil();
				toil.initAction = delegate()
				{
					Pawn actor = toil.actor;
					Job curJob = actor.jobs.curJob;
					actor.pather.StartPath(curJob.targetC, PathEndMode.OnCell);
				};
				toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
				toil.FailOnDestroyedNullOrForbidden(TargetIndex.B);
				toil.AddFailCondition(delegate
				{
					Pawn pawn = (Pawn)toil.actor.jobs.curJob.targetB.Thing;
					return !pawn.IsPrisonerOfColony || !pawn.guest.CanBeBroughtFood;
				});
				this.$current = toil;
				if (!this.$disposing)
				{
					this.$PC = 7;
				}
				return true;
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
				JobDriver_FoodDeliver.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_FoodDeliver.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil toil;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Pawn actor = this.toil.actor;
					Job curJob = actor.jobs.curJob;
					actor.pather.StartPath(curJob.targetC, PathEndMode.OnCell);
				}

				internal bool <>m__1()
				{
					Pawn pawn = (Pawn)this.toil.actor.jobs.curJob.targetB.Thing;
					return !pawn.IsPrisonerOfColony || !pawn.guest.CanBeBroughtFood;
				}
			}

			private sealed class <MakeNewToils>c__AnonStorey2
			{
				internal Toil toil;

				internal JobDriver_FoodDeliver.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey2()
				{
				}

				internal void <>m__0()
				{
					Thing thing;
					this.<>f__ref$0.$this.pawn.carryTracker.TryDropCarriedThing(this.toil.actor.jobs.curJob.targetC.Cell, ThingPlaceMode.Direct, out thing, null);
				}
			}
		}
	}
}
