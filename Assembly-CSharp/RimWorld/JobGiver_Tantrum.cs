using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000115 RID: 277
	public class JobGiver_Tantrum : ThinkNode_JobGiver
	{
		// Token: 0x060005A7 RID: 1447 RVA: 0x0003CB18 File Offset: 0x0003AF18
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
						return null;
					}
					if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
					{
						return null;
					}
				}
				result = new Job(JobDefOf.AttackMelee, mentalState_Tantrum.target)
				{
					maxNumMeleeAttacks = 1,
					verbToUse = verbToUse
				};
			}
			return result;
		}
	}
}
