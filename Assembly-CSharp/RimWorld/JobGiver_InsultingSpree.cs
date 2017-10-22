using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_InsultingSpree : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = pawn.MentalState as MentalState_InsultingSpree;
			return (mentalState_InsultingSpree != null && mentalState_InsultingSpree.target != null && pawn.CanReach((Thing)mentalState_InsultingSpree.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)) ? new Job(JobDefOf.Insult, (Thing)mentalState_InsultingSpree.target) : null;
		}
	}
}
