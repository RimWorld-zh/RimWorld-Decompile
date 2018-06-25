using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SpectateDutySpectateRect : ThinkNode_JobGiver
	{
		public JobGiver_SpectateDutySpectateRect()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			Job result;
			IntVec3 c;
			if (duty == null)
			{
				result = null;
			}
			else if (!SpectatorCellFinder.TryFindSpectatorCellFor(pawn, duty.spectateRect, pawn.Map, out c, duty.spectateRectAllowedSides, 1, null))
			{
				result = null;
			}
			else
			{
				IntVec3 centerCell = duty.spectateRect.CenterCell;
				Building edifice = c.GetEdifice(pawn.Map);
				if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable && pawn.CanReserve(edifice, 1, -1, null, false))
				{
					result = new Job(JobDefOf.SpectateCeremony, edifice, centerCell);
				}
				else
				{
					result = new Job(JobDefOf.SpectateCeremony, c, centerCell);
				}
			}
			return result;
		}
	}
}
