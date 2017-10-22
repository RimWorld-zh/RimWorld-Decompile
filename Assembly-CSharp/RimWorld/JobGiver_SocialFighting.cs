using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SocialFighting : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.RaceProps.Humanlike && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				result = null;
			}
			else
			{
				Pawn otherPawn = ((MentalState_SocialFighting)pawn.MentalState).otherPawn;
				Verb verbToUse = default(Verb);
				if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
				{
					result = null;
				}
				else
				{
					Job job = new Job(JobDefOf.SocialFight, (Thing)otherPawn);
					job.maxNumMeleeAttacks = 1;
					job.verbToUse = verbToUse;
					result = job;
				}
			}
			return result;
		}
	}
}
