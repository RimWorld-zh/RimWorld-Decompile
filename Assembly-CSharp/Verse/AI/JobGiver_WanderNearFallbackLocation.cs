using System;

namespace Verse.AI
{
	// Token: 0x02000AD6 RID: 2774
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x06003D8D RID: 15757 RVA: 0x00206418 File Offset: 0x00204818
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x00206440 File Offset: 0x00204840
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
