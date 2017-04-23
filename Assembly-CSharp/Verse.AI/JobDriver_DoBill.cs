using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				IBillGiver billGiver = this.pawn.jobs.curJob.GetTarget(TargetIndex.A).Thing as IBillGiver;
				if (billGiver == null)
				{
					throw new InvalidOperationException("DoBill on non-Billgiver.");
				}
				return billGiver;
			}
		}

		public override string GetReport()
		{
			if (this.pawn.jobs.curJob.RecipeDef != null)
			{
				return base.ReportStringProcessed(this.pawn.jobs.curJob.RecipeDef.jobString);
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

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DoBill.<MakeNewToils>c__Iterator1B1 <MakeNewToils>c__Iterator1B = new JobDriver_DoBill.<MakeNewToils>c__Iterator1B1();
			<MakeNewToils>c__Iterator1B.<>f__this = this;
			JobDriver_DoBill.<MakeNewToils>c__Iterator1B1 expr_0E = <MakeNewToils>c__Iterator1B;
			expr_0E.$PC = -2;
			return expr_0E;
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
					return;
				}
				if (actor.carryTracker.Full)
				{
					return;
				}
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
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
									List<int> expr_1B3 = countQueue = curJob.countQueue;
									int num3;
									int expr_1B7 = num3 = i;
									num3 = countQueue[num3];
									expr_1B3[expr_1B7] = num3 - num2;
									if (curJob.countQueue[i] == 0)
									{
										curJob.countQueue.RemoveAt(i);
										targetQueue.RemoveAt(i);
									}
									actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
									return;
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
