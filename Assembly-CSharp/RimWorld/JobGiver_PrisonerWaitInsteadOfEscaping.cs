using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_PrisonerWaitInsteadOfEscaping : JobGiver_Wander
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.guest == null || !pawn.guest.ShouldWaitInsteadOfEscaping)
			{
				result = null;
			}
			else
			{
				Room room = pawn.GetRoom(RegionType.Set_Passable);
				if (room != null && room.isPrisonCell)
				{
					result = null;
				}
				else
				{
					IntVec3 spotToWaitInsteadOfEscaping = pawn.guest.spotToWaitInsteadOfEscaping;
					if (!spotToWaitInsteadOfEscaping.IsValid || !pawn.CanReach(spotToWaitInsteadOfEscaping, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out spotToWaitInsteadOfEscaping))
						{
							result = null;
							goto IL_009f;
						}
						pawn.guest.spotToWaitInsteadOfEscaping = spotToWaitInsteadOfEscaping;
					}
					result = base.TryGiveJob(pawn);
				}
			}
			goto IL_009f;
			IL_009f:
			return result;
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.guest.spotToWaitInsteadOfEscaping;
		}
	}
}
