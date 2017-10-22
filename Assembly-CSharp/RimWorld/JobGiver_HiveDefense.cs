using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_HiveDefense : JobGiver_AIFightEnemies
	{
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			return (hive == null || !hive.Spawned) ? pawn.Position : hive.Position;
		}

		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}

		protected override Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(enemyTarget);
			job.attackDoorIfTargetLost = true;
			return job;
		}
	}
}
