using System;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		[CompilerGenerated]
		private static Func<Pawn, IntVec3, IntVec3, bool> <>f__am$cache0;

		public JobGiver_WanderOwnRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(300, 600);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

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

		[CompilerGenerated]
		private static bool <JobGiver_WanderOwnRoom>m__0(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			return WanderRoomUtility.IsValidWanderDest(pawn, loc, root);
		}
	}
}
