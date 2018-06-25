using System;

namespace Verse.AI
{
	// Token: 0x02000AD9 RID: 2777
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x06003D91 RID: 15761 RVA: 0x00206824 File Offset: 0x00204C24
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x0020684C File Offset: 0x00204C4C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
