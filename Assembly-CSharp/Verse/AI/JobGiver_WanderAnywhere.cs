using System;

namespace Verse.AI
{
	// Token: 0x02000AD3 RID: 2771
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x06003D83 RID: 15747 RVA: 0x00205C86 File Offset: 0x00204086
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x00205CB4 File Offset: 0x002040B4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
