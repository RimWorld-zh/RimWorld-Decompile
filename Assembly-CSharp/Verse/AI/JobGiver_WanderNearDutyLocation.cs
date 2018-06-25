using System;

namespace Verse.AI
{
	// Token: 0x02000AD8 RID: 2776
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x06003D8F RID: 15759 RVA: 0x002067CC File Offset: 0x00204BCC
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x002067F4 File Offset: 0x00204BF4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
