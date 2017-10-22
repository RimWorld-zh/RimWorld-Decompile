namespace Verse.AI
{
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		public JobGiver_RunRandom()
		{
			base.wanderRadius = 7f;
			base.ticksBetweenWandersRange = new IntRange(5, 10);
			base.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
