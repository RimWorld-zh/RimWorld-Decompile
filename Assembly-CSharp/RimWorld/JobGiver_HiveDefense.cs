using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000A0 RID: 160
	public class JobGiver_HiveDefense : JobGiver_AIFightEnemies
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x00030598 File Offset: 0x0002E998
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

		// Token: 0x06000406 RID: 1030 RVA: 0x000305EC File Offset: 0x0002E9EC
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00030614 File Offset: 0x0002EA14
		protected override Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(enemyTarget);
			job.attackDoorIfTargetLost = true;
			return job;
		}
	}
}
