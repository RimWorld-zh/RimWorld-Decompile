using System;

namespace Verse.AI
{
	// Token: 0x02000ACF RID: 2767
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x06003D7E RID: 15742 RVA: 0x00205FAA File Offset: 0x002043AA
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x00205FD8 File Offset: 0x002043D8
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
