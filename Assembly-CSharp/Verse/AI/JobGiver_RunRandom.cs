using System;

namespace Verse.AI
{
	// Token: 0x02000ADB RID: 2779
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x06003D97 RID: 15767 RVA: 0x0020695C File Offset: 0x00204D5C
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x00206988 File Offset: 0x00204D88
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
