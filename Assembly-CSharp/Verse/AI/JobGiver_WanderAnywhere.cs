using System;

namespace Verse.AI
{
	// Token: 0x02000AD1 RID: 2769
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x06003D82 RID: 15746 RVA: 0x002060D6 File Offset: 0x002044D6
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x00206104 File Offset: 0x00204504
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
