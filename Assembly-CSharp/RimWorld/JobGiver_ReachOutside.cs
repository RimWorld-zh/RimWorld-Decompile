using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_ReachOutside : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c = default(IntVec3);
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out c))
			{
				return null;
			}
			return new Job(JobDefOf.Goto, c);
		}
	}
}
