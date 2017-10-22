using RimWorld;

namespace Verse.AI
{
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.WaitDowned);
		}
	}
}
