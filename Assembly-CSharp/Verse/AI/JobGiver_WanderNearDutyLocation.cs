using System;

namespace Verse.AI
{
	// Token: 0x02000AD7 RID: 2775
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x06003D8F RID: 15759 RVA: 0x002064EC File Offset: 0x002048EC
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x00206514 File Offset: 0x00204914
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
