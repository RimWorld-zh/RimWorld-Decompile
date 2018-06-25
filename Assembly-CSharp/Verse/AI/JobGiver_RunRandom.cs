using System;

namespace Verse.AI
{
	// Token: 0x02000ADA RID: 2778
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x06003D97 RID: 15767 RVA: 0x0020667C File Offset: 0x00204A7C
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x002066A8 File Offset: 0x00204AA8
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
