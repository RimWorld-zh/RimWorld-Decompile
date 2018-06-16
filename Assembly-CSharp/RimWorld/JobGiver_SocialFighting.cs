using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000114 RID: 276
	public class JobGiver_SocialFighting : ThinkNode_JobGiver
	{
		// Token: 0x060005A5 RID: 1445 RVA: 0x0003CA84 File Offset: 0x0003AE84
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
