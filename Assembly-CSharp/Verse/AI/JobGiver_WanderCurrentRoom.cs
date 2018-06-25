using System;

namespace Verse.AI
{
	// Token: 0x02000AD2 RID: 2770
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		// Token: 0x06003D84 RID: 15748 RVA: 0x00206120 File Offset: 0x00204520
		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x0020617C File Offset: 0x0020457C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
