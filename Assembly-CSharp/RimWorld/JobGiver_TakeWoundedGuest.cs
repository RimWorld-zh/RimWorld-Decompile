using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_TakeWoundedGuest : ThinkNode_JobGiver
	{
		public JobGiver_TakeWoundedGuest()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			Job result;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = KidnapAIUtility.ReachableWoundedGuest(pawn);
				if (pawn2 == null)
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Kidnap)
					{
						targetA = pawn2,
						targetB = c,
						count = 1
					};
				}
			}
			return result;
		}
	}
}
