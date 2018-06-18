using System;

namespace Verse.AI
{
	// Token: 0x02000ADC RID: 2780
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x06003D98 RID: 15768 RVA: 0x0020622C File Offset: 0x0020462C
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x00206258 File Offset: 0x00204658
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
