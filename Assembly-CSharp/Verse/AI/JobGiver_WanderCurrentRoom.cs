using System;

namespace Verse.AI
{
	// Token: 0x02000AD0 RID: 2768
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		// Token: 0x06003D80 RID: 15744 RVA: 0x00205FF4 File Offset: 0x002043F4
		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x00206050 File Offset: 0x00204450
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
