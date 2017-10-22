using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_LayEgg : ThinkNode_JobGiver
	{
		private const float LayRadius = 5f;

		protected override Job TryGiveJob(Pawn pawn)
		{
			CompEggLayer compEggLayer = pawn.TryGetComp<CompEggLayer>();
			Job result;
			if (compEggLayer == null || !compEggLayer.CanLayNow)
			{
				result = null;
			}
			else
			{
				IntVec3 c = RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 5f, null, Danger.Some);
				result = new Job(JobDefOf.LayEgg, c);
			}
			return result;
		}
	}
}
