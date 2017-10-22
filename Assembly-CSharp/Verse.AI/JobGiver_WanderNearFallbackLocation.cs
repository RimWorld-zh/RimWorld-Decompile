namespace Verse.AI
{
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		public JobGiver_WanderNearFallbackLocation()
		{
			base.wanderRadius = 7f;
			base.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
