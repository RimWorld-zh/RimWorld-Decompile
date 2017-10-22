using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	public class JobDriver_DoBill : JobDriver
	{
		public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

		public const TargetIndex BillGiverInd = TargetIndex.A;

		public const TargetIndex IngredientInd = TargetIndex.B;

		public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;

		public float workLeft;

		public int billStartTick;

		public int ticksSpentDoingRecipeWork;

		public IBillGiver BillGiver
		{
			get
			{
				IBillGiver billGiver = base.pawn.jobs.curJob.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver == null)
				{
					throw new InvalidOperationException("DoBill on non-Billgiver.");
				}
				return billGiver;
			}
		}

		public override string GetReport()
		{
			if (base.pawn.jobs.curJob.RecipeDef != null)
			{
				return base.ReportStringProcessed(base.pawn.jobs.curJob.RecipeDef.jobString);
			}
			return base.GetReport();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.billStartTick, "billStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksSpentDoingRecipeWork, "ticksSpentDoingRecipeWork", 0, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.AddEndCondition((Func<JobCondition>)delegate
			{
				Thing thing = ((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_0073: stateMachine*/)._003C_003Ef__this.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
				if (thing is Building && !thing.Spawned)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn((Func<bool>)delegate
			{
				IBillGiver billGiver = ((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_0097: stateMachine*/)._003C_003Ef__this.pawn.jobs.curJob.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver != null)
				{
					if (((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_0097: stateMachine*/)._003C_003Ef__this.pawn.jobs.curJob.bill.DeletedOrDereferenced)
					{
						return true;
					}
					if (!billGiver.CurrentlyUsable())
					{
						return true;
					}
				}
				return false;
			});
			Toil gotoBillGiver = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Reserve.ReserveQueue(TargetIndex.B, 1, -1, null);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_00fd: stateMachine*/)._003C_003Ef__this.CurJob.targetQueueB != null && ((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_00fd: stateMachine*/)._003C_003Ef__this.CurJob.targetQueueB.Count == 1)
					{
						UnfinishedThing unfinishedThing = ((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_00fd: stateMachine*/)._003C_003Ef__this.CurJob.targetQueueB[0].Thing as UnfinishedThing;
						if (unfinishedThing != null)
						{
							unfinishedThing.BoundBill = (Bill_ProductionWithUft)((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_00fd: stateMachine*/)._003C_003Ef__this.CurJob.bill;
						}
					}
				}
			};
			yield return Toils_Jump.JumpIf(gotoBillGiver, (Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_012d: stateMachine*/)._003C_003Ef__this.CurJob.GetTargetQueue(TargetIndex.B).NullOrEmpty()));
			Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B);
			yield return extract;
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return getToHaulTarget;
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, true, false);
			yield return JobDriver_DoBill.JumpToCollectNextIntoHandsForBill(getToHaulTarget, TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDestroyedOrNull(TargetIndex.B);
			Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
			yield return findPlaceTarget;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false);
			yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, extract);
			yield return gotoBillGiver;
			yield return Toils_Recipe.MakeUnfinishedThingIfNeeded();
			yield return Toils_Recipe.DoRecipeWork().FailOnDespawnedOrForbiddenPlacedThings().FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_Recipe.FinishRecipeAndStartStoringProduct();
			if (base.CurJob.RecipeDef.products.NullOrEmpty() && base.CurJob.RecipeDef.specialProducts.NullOrEmpty())
				yield break;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Bill_Production bill_Production = ((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_037f: stateMachine*/)._003Crecount_003E__6.actor.jobs.curJob.bill as Bill_Production;
					if (bill_Production != null && bill_Production.repeatMode == BillRepeatModeDefOf.TargetCount)
					{
						((_003CMakeNewToils_003Ec__Iterator1B9)/*Error near IL_037f: stateMachine*/)._003C_003Ef__this.Map.resourceCounter.UpdateResourceCounts();
					}
				}
			};
		}

		private static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Log.Error("JumpToAlsoCollectTargetInQueue run on " + actor + " who is not carrying something.");
				}
				else if (!actor.carryTracker.Full)
				{
					Job curJob = actor.jobs.curJob;
					List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
					if (!targetQueue.NullOrEmpty())
					{
						int num = 0;
						int a;
						while (true)
						{
							if (num < targetQueue.Count)
							{
								if (GenAI.CanUseItemForWork(actor, targetQueue[num].Thing) && targetQueue[num].Thing.CanStackWith(actor.carryTracker.CarriedThing) && !((float)(actor.Position - targetQueue[num].Thing.Position).LengthHorizontalSquared > 64.0))
								{
									int num2 = (actor.carryTracker.CarriedThing != null) ? actor.carryTracker.CarriedThing.stackCount : 0;
									a = curJob.countQueue[num];
									a = Mathf.Min(a, targetQueue[num].Thing.def.stackLimit - num2);
									a = Mathf.Min(a, actor.carryTracker.AvailableStackSpace(targetQueue[num].Thing.def));
									if (a > 0)
										break;
								}
								num++;
								continue;
							}
							return;
						}
						curJob.count = a;
						curJob.SetTarget(ind, targetQueue[num].Thing);
						List<int> countQueue;
						List<int> obj = countQueue = curJob.countQueue;
						int index;
						int index2 = index = num;
						index = countQueue[index];
						obj[index2] = index - a;
						if (curJob.countQueue[num] == 0)
						{
							curJob.countQueue.RemoveAt(num);
							targetQueue.RemoveAt(num);
						}
						actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
					}
				}
			};
			return toil;
		}
	}
}
