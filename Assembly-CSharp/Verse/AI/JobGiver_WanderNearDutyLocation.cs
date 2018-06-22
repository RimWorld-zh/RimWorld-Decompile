using System;

namespace Verse.AI
{
	// Token: 0x02000AD5 RID: 2773
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x06003D8B RID: 15755 RVA: 0x002063C0 File Offset: 0x002047C0
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x002063E8 File Offset: 0x002047E8
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
