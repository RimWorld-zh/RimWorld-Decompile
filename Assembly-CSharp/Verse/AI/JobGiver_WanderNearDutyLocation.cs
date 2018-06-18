using System;

namespace Verse.AI
{
	// Token: 0x02000AD9 RID: 2777
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x06003D90 RID: 15760 RVA: 0x0020609C File Offset: 0x0020449C
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x002060C4 File Offset: 0x002044C4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
