using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000A3 RID: 163
	public class JobGiver_WanderHive : JobGiver_Wander
	{
		// Token: 0x0600040D RID: 1037 RVA: 0x00030A4C File Offset: 0x0002EE4C
		public JobGiver_WanderHive()
		{
			this.wanderRadius = 7.5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00030A74 File Offset: 0x0002EE74
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			IntVec3 position;
			if (hive == null || !hive.Spawned)
			{
				position = pawn.Position;
			}
			else
			{
				position = hive.Position;
			}
			return position;
		}
	}
}
