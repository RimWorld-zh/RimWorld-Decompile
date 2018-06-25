using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_HiveDefense : JobGiver_AIFightEnemies
	{
		public JobGiver_HiveDefense()
		{
		}

		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			IntVec3 position;
			if (hive != null && hive.Spawned)
			{
				position = hive.Position;
			}
			else
			{
				position = pawn.Position;
			}
			return position;
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
