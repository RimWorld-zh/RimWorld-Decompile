using System;

namespace Verse.AI
{
	// Token: 0x02000AD8 RID: 2776
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x06003D91 RID: 15761 RVA: 0x00206544 File Offset: 0x00204944
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x0020656C File Offset: 0x0020496C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
