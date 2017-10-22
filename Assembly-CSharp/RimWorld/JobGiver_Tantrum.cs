using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Tantrum : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Tantrum mentalState_Tantrum = pawn.MentalState as MentalState_Tantrum;
			Job result;
			if (mentalState_Tantrum == null || mentalState_Tantrum.target == null || !pawn.CanReach(mentalState_Tantrum.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = null;
			}
			else
			{
				Verb verbToUse = null;
				Pawn pawn2 = mentalState_Tantrum.target as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.Downed)
					{
						result = null;
						goto IL_00ab;
					}
					if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
					{
						result = null;
						goto IL_00ab;
					}
				}
				Job job = new Job(JobDefOf.AttackMelee, mentalState_Tantrum.target);
				job.maxNumMeleeAttacks = 1;
				job.verbToUse = verbToUse;
				result = job;
			}
			goto IL_00ab;
			IL_00ab:
			return result;
		}
	}
}
