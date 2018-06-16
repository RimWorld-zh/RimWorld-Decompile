using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000EB RID: 235
	public class JobGiver_PrisonerWaitInsteadOfEscaping : JobGiver_Wander
	{
		// Token: 0x06000507 RID: 1287 RVA: 0x00037EDC File Offset: 0x000362DC
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
							return null;
						}
						pawn.guest.spotToWaitInsteadOfEscaping = spotToWaitInsteadOfEscaping;
					}
					result = base.TryGiveJob(pawn);
				}
			}
			return result;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00037F8C File Offset: 0x0003638C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.guest.spotToWaitInsteadOfEscaping;
		}
	}
}
