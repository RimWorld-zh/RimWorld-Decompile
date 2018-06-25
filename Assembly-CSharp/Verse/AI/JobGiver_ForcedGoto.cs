using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC5 RID: 2757
	public class JobGiver_ForcedGoto : ThinkNode_JobGiver
	{
		// Token: 0x06003D51 RID: 15697 RVA: 0x00205A08 File Offset: 0x00203E08
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 forcedGotoPosition = pawn.mindState.forcedGotoPosition;
			Job result;
			if (!forcedGotoPosition.IsValid)
			{
				result = null;
			}
			else if (!pawn.CanReach(forcedGotoPosition, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.Goto, forcedGotoPosition)
				{
					locomotionUrgency = LocomotionUrgency.Walk
				};
			}
			return result;
		}
	}
}
