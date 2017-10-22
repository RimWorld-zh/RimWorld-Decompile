using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_DoLovin : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (Find.TickManager.TicksGame < pawn.mindState.canLovinTick)
			{
				result = null;
			}
			else if (pawn.CurrentBed() == null || pawn.CurrentBed().Medical || !pawn.health.capacities.CanBeAwake)
			{
				result = null;
			}
			else
			{
				Pawn partnerInMyBed = LovePartnerRelationUtility.GetPartnerInMyBed(pawn);
				result = ((partnerInMyBed == null || !partnerInMyBed.health.capacities.CanBeAwake || Find.TickManager.TicksGame < partnerInMyBed.mindState.canLovinTick) ? null : ((pawn.CanReserve((Thing)partnerInMyBed, 1, -1, null, false) && partnerInMyBed.CanReserve((Thing)pawn, 1, -1, null, false)) ? new Job(JobDefOf.Lovin, (Thing)partnerInMyBed, (Thing)pawn.CurrentBed()) : null));
			}
			return result;
		}
	}
}
