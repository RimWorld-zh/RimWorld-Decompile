using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SelfTend : ThinkNode_JobGiver
	{
		public JobGiver_SelfTend()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.RaceProps.Humanlike || !pawn.health.HasHediffsNeedingTend(false) || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || pawn.InAggroMentalState)
			{
				result = null;
			}
			else if (pawn.IsColonist && pawn.story.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.TendPatient, pawn)
				{
					endAfterTendedOnce = true
				};
			}
			return result;
		}
	}
}
