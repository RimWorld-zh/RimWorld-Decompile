using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_ReactToCloseMeleeThreat : ThinkNode_JobGiver
	{
		private const int MaxMeleeChaseTicks = 200;

		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn meleeThreat = pawn.mindState.meleeThreat;
			Job result;
			if (meleeThreat == null)
			{
				result = null;
			}
			else if (this.IsHunting(pawn, meleeThreat))
			{
				result = null;
			}
			else if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				result = null;
			}
			else if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse && pawn.playerSettings.hostilityResponse != HostilityResponseMode.Attack)
			{
				result = null;
			}
			else if (!pawn.mindState.MeleeThreatStillThreat)
			{
				pawn.mindState.meleeThreat = null;
				result = null;
			}
			else if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				result = null;
			}
			else
			{
				Job job = new Job(JobDefOf.AttackMelee, (Thing)meleeThreat);
				job.maxNumMeleeAttacks = 1;
				job.expiryInterval = 200;
				result = job;
			}
			return result;
		}

		private bool IsHunting(Pawn pawn, Pawn prey)
		{
			bool result;
			if (pawn.CurJob == null)
			{
				result = false;
			}
			else
			{
				JobDriver_Hunt jobDriver_Hunt = pawn.jobs.curDriver as JobDriver_Hunt;
				if (jobDriver_Hunt != null)
				{
					result = (jobDriver_Hunt.Victim == prey);
				}
				else
				{
					JobDriver_PredatorHunt jobDriver_PredatorHunt = pawn.jobs.curDriver as JobDriver_PredatorHunt;
					result = (jobDriver_PredatorHunt != null && jobDriver_PredatorHunt.Prey == prey);
				}
			}
			return result;
		}
	}
}
