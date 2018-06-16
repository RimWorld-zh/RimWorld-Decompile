using System;

namespace Verse.AI
{
	// Token: 0x02000ADA RID: 2778
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x06003D90 RID: 15760 RVA: 0x00206020 File Offset: 0x00204420
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x00206048 File Offset: 0x00204448
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
