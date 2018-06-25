using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A3F RID: 2623
	public class JobDriver_DoBill : JobDriver
	{
		// Token: 0x04002513 RID: 9491
		public float workLeft;

		// Token: 0x04002514 RID: 9492
		public int billStartTick;

		// Token: 0x04002515 RID: 9493
		public int ticksSpentDoingRecipeWork;

		// Token: 0x04002516 RID: 9494
		public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

		// Token: 0x04002517 RID: 9495
		public const TargetIndex BillGiverInd = TargetIndex.A;

		// Token: 0x04002518 RID: 9496
		public const TargetIndex IngredientInd = TargetIndex.B;

		// Token: 0x04002519 RID: 9497
		public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;

		// Token: 0x06003A29 RID: 14889 RVA: 0x001EC89C File Offset: 0x001EAC9C
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

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06003A2A RID: 14890 RVA: 0x001EC8E4 File Offset: 0x001EACE4
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

		// Token: 0x06003A2B RID: 14891 RVA: 0x001EC928 File Offset: 0x001EAD28
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.billStartTick, "billStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksSpentDoingRecipeWork, "ticksSpentDoingRecipeWork", 0, false);
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x001EC978 File Offset: 0x001EAD78
		public override bool TryMakePreToilReservations()
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x001EC9D0 File Offset: 0x001EADD0
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

		// Token: 0x06003A2E RID: 14894 RVA: 0x001EC9FC File Offset: 0x001EADFC
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
	}
}
