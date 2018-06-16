using System;

namespace Verse.AI
{
	// Token: 0x02000ADC RID: 2780
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x06003D96 RID: 15766 RVA: 0x00206158 File Offset: 0x00204558
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x00206184 File Offset: 0x00204584
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
