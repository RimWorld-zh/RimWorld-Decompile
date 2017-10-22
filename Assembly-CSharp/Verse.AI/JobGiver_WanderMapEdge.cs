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
			IntVec3 intVec = default(IntVec3);
			return (!RCellFinder.TryFindBestExitSpot(pawn, out intVec, TraverseMode.ByPawn)) ? pawn.Position : intVec;
		}
	}
}
