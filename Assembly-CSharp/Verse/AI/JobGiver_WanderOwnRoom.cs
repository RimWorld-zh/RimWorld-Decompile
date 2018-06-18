using System;

namespace Verse.AI
{
	// Token: 0x02000AD8 RID: 2776
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		// Token: 0x06003D8D RID: 15757 RVA: 0x00205FE4 File Offset: 0x002043E4
		public JobGiver_WanderOwnRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(300, 600);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x00206044 File Offset: 0x00204444
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
