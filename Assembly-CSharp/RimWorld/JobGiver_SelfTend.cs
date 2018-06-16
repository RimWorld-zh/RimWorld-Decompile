using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000F0 RID: 240
	public class JobGiver_SelfTend : ThinkNode_JobGiver
	{
		// Token: 0x06000517 RID: 1303 RVA: 0x000386F8 File Offset: 0x00036AF8
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
