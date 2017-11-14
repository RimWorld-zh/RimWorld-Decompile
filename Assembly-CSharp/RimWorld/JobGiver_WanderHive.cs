using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_WanderHive : JobGiver_Wander
	{
		public JobGiver_WanderHive()
		{
			base.wanderRadius = 7.5f;
			base.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive != null && hive.Spawned)
			{
				return hive.Position;
			}
			return pawn.Position;
		}
	}
}
