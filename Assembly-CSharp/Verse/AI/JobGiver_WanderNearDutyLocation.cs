using System;

namespace Verse.AI
{
	// Token: 0x02000AD9 RID: 2777
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x06003D8E RID: 15758 RVA: 0x00205FC8 File Offset: 0x002043C8
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x00205FF0 File Offset: 0x002043F0
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
