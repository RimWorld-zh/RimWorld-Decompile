using System;

namespace Verse.AI
{
	// Token: 0x02000AD4 RID: 2772
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		// Token: 0x06003D85 RID: 15749 RVA: 0x00205CD0 File Offset: 0x002040D0
		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x00205D2C File Offset: 0x0020412C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
