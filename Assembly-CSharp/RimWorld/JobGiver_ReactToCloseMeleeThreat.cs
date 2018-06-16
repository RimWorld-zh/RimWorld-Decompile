using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000EC RID: 236
	public class JobGiver_ReactToCloseMeleeThreat : ThinkNode_JobGiver
	{
		// Token: 0x0600050A RID: 1290 RVA: 0x00037FB4 File Offset: 0x000363B4
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
				result = new Job(JobDefOf.AttackMelee, meleeThreat)
				{
					maxNumMeleeAttacks = 1,
					expiryInterval = 200
				};
			}
			return result;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000380A8 File Offset: 0x000364A8
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

		// Token: 0x040002CA RID: 714
		private const int MaxMeleeChaseTicks = 200;
	}
}
