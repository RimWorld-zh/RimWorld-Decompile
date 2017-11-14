namespace Verse.AI
{
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		public JobGiver_WanderOwnRoom()
		{
			base.wanderRadius = 7f;
			base.ticksBetweenWandersRange = new IntRange(300, 600);
			base.locomotionUrgency = LocomotionUrgency.Amble;
			base.wanderDestValidator = ((Pawn pawn, IntVec3 loc) => WanderRoomUtility.IsValidWanderDest(pawn, loc, this.GetWanderRoot(pawn)));
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
