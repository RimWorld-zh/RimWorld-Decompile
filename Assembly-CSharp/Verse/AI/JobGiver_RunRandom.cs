using System;

namespace Verse.AI
{
	// Token: 0x02000AD8 RID: 2776
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x06003D93 RID: 15763 RVA: 0x00206550 File Offset: 0x00204950
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0020657C File Offset: 0x0020497C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
