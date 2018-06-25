using System;

namespace Verse.AI
{
	// Token: 0x02000AD2 RID: 2770
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x06003D82 RID: 15746 RVA: 0x002063B6 File Offset: 0x002047B6
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x002063E4 File Offset: 0x002047E4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
