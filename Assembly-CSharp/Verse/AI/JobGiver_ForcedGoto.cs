using System;
using RimWorld;

namespace Verse.AI
{
	public class JobGiver_ForcedGoto : ThinkNode_JobGiver
	{
		public JobGiver_ForcedGoto()
		{
		}

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
