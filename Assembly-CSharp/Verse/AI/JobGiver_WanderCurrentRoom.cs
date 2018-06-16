using System;

namespace Verse.AI
{
	// Token: 0x02000AD4 RID: 2772
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		// Token: 0x06003D83 RID: 15747 RVA: 0x00205BFC File Offset: 0x00203FFC
		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x00205C58 File Offset: 0x00204058
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
