using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000D2 RID: 210
	public class JobGiver_GotoTravelDestination : ThinkNode_JobGiver
	{
		// Token: 0x040002A0 RID: 672
		private LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x040002A1 RID: 673
		private Danger maxDanger = Danger.Some;

		// Token: 0x040002A2 RID: 674
		private int jobMaxDuration = 999999;

		// Token: 0x040002A3 RID: 675
		private bool exactCell;

		// Token: 0x040002A4 RID: 676
		private IntRange WaitTicks = new IntRange(30, 80);

		// Token: 0x060004B7 RID: 1207 RVA: 0x00035254 File Offset: 0x00033654
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GotoTravelDestination jobGiver_GotoTravelDestination = (JobGiver_GotoTravelDestination)base.DeepCopy(resolve);
			jobGiver_GotoTravelDestination.locomotionUrgency = this.locomotionUrgency;
			jobGiver_GotoTravelDestination.maxDanger = this.maxDanger;
			jobGiver_GotoTravelDestination.jobMaxDuration = this.jobMaxDuration;
			jobGiver_GotoTravelDestination.exactCell = this.exactCell;
			return jobGiver_GotoTravelDestination;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000352A8 File Offset: 0x000336A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			Job result;
			if (pawn.mindState.nextMoveOrderIsWait && !this.exactCell)
			{
				result = new Job(JobDefOf.Wait_Wander)
				{
					expiryInterval = this.WaitTicks.RandomInRange
				};
			}
			else
			{
				IntVec3 cell = pawn.mindState.duty.focus.Cell;
				if (!pawn.CanReach(cell, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, TraverseMode.ByPawn))
				{
					result = null;
				}
				else if (this.exactCell && pawn.Position == cell)
				{
					result = null;
				}
				else
				{
					IntVec3 c = cell;
					if (!this.exactCell)
					{
						c = CellFinder.RandomClosewalkCellNear(cell, pawn.Map, 6, null);
					}
					result = new Job(JobDefOf.Goto, c)
					{
						locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency),
						expiryInterval = this.jobMaxDuration
					};
				}
			}
			return result;
		}
	}
}
