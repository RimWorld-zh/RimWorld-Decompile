using System;

namespace Verse.AI
{
	// Token: 0x02000AD4 RID: 2772
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		// Token: 0x06003D88 RID: 15752 RVA: 0x00206308 File Offset: 0x00204708
		public JobGiver_WanderOwnRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(300, 600);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x00206368 File Offset: 0x00204768
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			MentalState_WanderOwnRoom mentalState_WanderOwnRoom = pawn.MentalState as MentalState_WanderOwnRoom;
			IntVec3 result;
			if (mentalState_WanderOwnRoom != null)
			{
				result = mentalState_WanderOwnRoom.target;
			}
			else
			{
				result = pawn.Position;
			}
			return result;
		}
	}
}
