using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000CC RID: 204
	public class JobGiver_SpectateDutySpectateRect : ThinkNode_JobGiver
	{
		// Token: 0x060004A6 RID: 1190 RVA: 0x00034D70 File Offset: 0x00033170
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
