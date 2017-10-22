using System;

namespace Verse.AI
{
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		public JobGiver_WanderCurrentRoom()
		{
			base.wanderRadius = 7f;
			base.ticksBetweenWandersRange = new IntRange(125, 200);
			base.locomotionUrgency = LocomotionUrgency.Amble;
			base.wanderDestValidator = (Func<Pawn, IntVec3, bool>)((Pawn pawn, IntVec3 loc) => WanderRoomUtility.IsValidWanderDest(pawn, loc, this.GetWanderRoot(pawn)));
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
