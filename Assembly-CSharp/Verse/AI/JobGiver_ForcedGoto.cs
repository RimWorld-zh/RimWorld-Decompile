using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC3 RID: 2755
	public class JobGiver_ForcedGoto : ThinkNode_JobGiver
	{
		// Token: 0x06003D4D RID: 15693 RVA: 0x002058DC File Offset: 0x00203CDC
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
