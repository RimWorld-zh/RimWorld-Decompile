using RimWorld;

namespace Verse.AI
{
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		public JobGiver_WanderMapEdge()
		{
			base.wanderRadius = 7f;
			base.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			IntVec3 result = default(IntVec3);
			if (RCellFinder.TryFindBestExitSpot(pawn, out result, TraverseMode.ByPawn))
			{
				return result;
			}
			return pawn.Position;
		}
	}
}
