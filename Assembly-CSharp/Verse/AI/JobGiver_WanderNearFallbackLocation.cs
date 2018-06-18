using System;

namespace Verse.AI
{
	// Token: 0x02000ADA RID: 2778
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x06003D92 RID: 15762 RVA: 0x002060F4 File Offset: 0x002044F4
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x0020611C File Offset: 0x0020451C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
