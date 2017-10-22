using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_DoLovin : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame < pawn.mindState.canLovinTick)
			{
				return null;
			}
			if (pawn.CurrentBed() != null && !pawn.CurrentBed().Medical && pawn.health.capacities.CanBeAwake)
			{
				Pawn partnerInMyBed = LovePartnerRelationUtility.GetPartnerInMyBed(pawn);
				if (partnerInMyBed != null && partnerInMyBed.health.capacities.CanBeAwake && Find.TickManager.TicksGame >= partnerInMyBed.mindState.canLovinTick)
				{
					if (pawn.CanReserve((Thing)partnerInMyBed, 1, -1, null, false) && partnerInMyBed.CanReserve((Thing)pawn, 1, -1, null, false))
					{
						pawn.mindState.awokeVoluntarily = true;
						partnerInMyBed.mindState.awokeVoluntarily = true;
						return new Job(JobDefOf.Lovin, (Thing)partnerInMyBed, (Thing)pawn.CurrentBed());
					}
					return null;
				}
				return null;
			}
			return null;
		}
	}
}
