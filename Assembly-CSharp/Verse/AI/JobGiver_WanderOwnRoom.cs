using System;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		[CompilerGenerated]
		private static Func<Pawn, IntVec3, IntVec3, bool> <>f__mg$cache0;

		public JobGiver_WanderOwnRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(300, 600);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			if (JobGiver_WanderOwnRoom.<>f__mg$cache0 == null)
			{
				JobGiver_WanderOwnRoom.<>f__mg$cache0 = new Func<Pawn, IntVec3, IntVec3, bool>(WanderRoomUtility.IsValidWanderDest);
			}
			this.wanderDestValidator = JobGiver_WanderOwnRoom.<>f__mg$cache0;
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			MentalState_WanderOwnRoom mentalState_WanderOwnRoom = pawn.MentalState as MentalState_WanderOwnRoom;
			if (mentalState_WanderOwnRoom != null)
			{
				return mentalState_WanderOwnRoom.target;
			}
			return pawn.Position;
		}
	}
}
