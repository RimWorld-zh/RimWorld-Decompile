using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_InducePrisonerToEscape : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = JailbreakerMentalStateUtility.FindPrisoner(pawn);
			Job result;
			if (pawn2 == null || !pawn.CanReach((Thing)pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = null;
			}
			else
			{
				Job job = new Job(JobDefOf.InducePrisonerToEscape, (Thing)pawn2);
				job.interaction = InteractionDefOf.SparkJailbreak;
				result = job;
			}
			return result;
		}
	}
}
