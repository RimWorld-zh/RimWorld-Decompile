using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_TakeWoundedGuest : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c = default(IntVec3);
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
					Job job = new Job(JobDefOf.Kidnap);
					job.targetA = (Thing)pawn2;
					job.targetB = c;
					job.count = 1;
					result = job;
				}
			}
			return result;
		}
	}
}
