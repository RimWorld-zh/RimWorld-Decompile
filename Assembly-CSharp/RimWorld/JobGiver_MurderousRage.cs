using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_MurderousRage : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_MurderousRage mentalState_MurderousRage = pawn.MentalState as MentalState_MurderousRage;
			Job result;
			if (mentalState_MurderousRage == null || mentalState_MurderousRage.target == null || !pawn.CanReach((Thing)mentalState_MurderousRage.target, PathEndMode.Touch, Danger.Deadly, true, TraverseMode.ByPawn))
			{
				result = null;
			}
			else
			{
				Job job = new Job(JobDefOf.AttackMelee, (Thing)mentalState_MurderousRage.target);
				job.canBash = true;
				job.killIncappedTarget = true;
				result = job;
			}
			return result;
		}
	}
}
