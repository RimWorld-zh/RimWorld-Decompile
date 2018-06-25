using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SocialFighting : ThinkNode_JobGiver
	{
		public JobGiver_SocialFighting()
		{
		}

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
				Verb verbToUse;
				if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.SocialFight, otherPawn)
					{
						maxNumMeleeAttacks = 1,
						verbToUse = verbToUse
					};
				}
			}
			return result;
		}
	}
}
