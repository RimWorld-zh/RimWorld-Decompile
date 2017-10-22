using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SpectateDutySpectateRect : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			Job result;
			IntVec3 c = default(IntVec3);
			if (duty == null)
			{
				result = null;
			}
			else if (!SpectatorCellFinder.TryFindSpectatorCellFor(pawn, duty.spectateRect, pawn.Map, out c, duty.spectateRectAllowedSides, 1, (List<IntVec3>)null))
			{
				result = null;
			}
			else
			{
				IntVec3 centerCell = duty.spectateRect.CenterCell;
				Building edifice = c.GetEdifice(pawn.Map);
				result = ((edifice == null || edifice.def.category != ThingCategory.Building || !edifice.def.building.isSittable || !pawn.CanReserve((Thing)edifice, 1, -1, null, false)) ? new Job(JobDefOf.SpectateCeremony, c, centerCell) : new Job(JobDefOf.SpectateCeremony, (Thing)edifice, centerCell));
			}
			return result;
		}
	}
}
