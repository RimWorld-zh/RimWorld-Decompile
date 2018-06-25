using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000111 RID: 273
	public class JobGiver_MurderousRage : ThinkNode_JobGiver
	{
		// Token: 0x0600059F RID: 1439 RVA: 0x0003C8F4 File Offset: 0x0003ACF4
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_MurderousRage mentalState_MurderousRage = pawn.MentalState as MentalState_MurderousRage;
			Job result;
			if (mentalState_MurderousRage == null || !mentalState_MurderousRage.IsTargetStillValidAndReachable())
			{
				result = null;
			}
			else
			{
				Thing spawnedParentOrMe = mentalState_MurderousRage.target.SpawnedParentOrMe;
				Job job = new Job(JobDefOf.AttackMelee, spawnedParentOrMe);
				job.canBash = true;
				job.killIncappedTarget = true;
				if (spawnedParentOrMe != mentalState_MurderousRage.target)
				{
					job.maxNumMeleeAttacks = 2;
				}
				result = job;
			}
			return result;
		}
	}
}
