using RimWorld;
using System;
using System.Collections.Generic;
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

		public IBillGiver BillGiver
		{
			get
			{
				IBillGiver billGiver = base.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver == null)
				{
					throw new InvalidOperationException("DoBill on non-Billgiver.");
				}
				return billGiver;
			}
		}

		public override string GetReport()
		{
			if (base.job.RecipeDef != null)
			{
				return base.ReportStringProcessed(base.job.RecipeDef.jobString);
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

		public override bool TryMakePreToilReservations()
		{
			base.pawn.ReserveAsManyAsPossible(base.job.GetTargetQueue(TargetIndex.B), base.job, 1, -1, null);
			return base.pawn.Reserve(base.job.GetTarget(TargetIndex.A), base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddEndCondition(delegate
			{
				Thing thing = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_006b: stateMachine*/)._0024this.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
				if (thing is Building && !thing.Spawned)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOn(delegate
			{
				IBillGiver billGiver = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_008f: stateMachine*/)._0024this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver != null)
				{
					if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_008f: stateMachine*/)._0024this.job.bill.DeletedOrDereferenced)
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
			Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return new Toil
			{
				initAction = delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00bf: stateMachine*/)._0024this.job.targetQueueB != null && ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00bf: stateMachine*/)._0024this.job.targetQueueB.Count == 1)
					{
						UnfinishedThing unfinishedThing = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00bf: stateMachine*/)._0024this.job.targetQueueB[0].Thing as UnfinishedThing;
						if (unfinishedThing != null)
						{
							unfinishedThing.BoundBill = (Bill_ProductionWithUft)((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00bf: stateMachine*/)._0024this.job.bill;
						}
					}
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
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
						int index;
						(countQueue = curJob.countQueue)[index = num] = countQueue[index] - a;
						if (curJob.countQueue[num] <= 0)
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
