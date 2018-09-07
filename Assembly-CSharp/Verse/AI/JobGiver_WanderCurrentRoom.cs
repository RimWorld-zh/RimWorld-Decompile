using System;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		[CompilerGenerated]
		private static Func<Pawn, IntVec3, IntVec3, bool> <>f__mg$cache0;

		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			if (JobGiver_WanderCurrentRoom.<>f__mg$cache0 == null)
			{
				JobGiver_WanderCurrentRoom.<>f__mg$cache0 = new Func<Pawn, IntVec3, IntVec3, bool>(WanderRoomUtility.IsValidWanderDest);
			}
			this.wanderDestValidator = JobGiver_WanderCurrentRoom.<>f__mg$cache0;
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
