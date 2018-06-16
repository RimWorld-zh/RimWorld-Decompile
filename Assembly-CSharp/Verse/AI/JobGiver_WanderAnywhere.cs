using System;

namespace Verse.AI
{
	// Token: 0x02000AD3 RID: 2771
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x06003D81 RID: 15745 RVA: 0x00205BB2 File Offset: 0x00203FB2
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x00205BE0 File Offset: 0x00203FE0
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
