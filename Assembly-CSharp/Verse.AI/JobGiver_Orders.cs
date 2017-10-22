using RimWorld;

namespace Verse.AI
{
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			return (!pawn.Drafted) ? null : new Job(JobDefOf.WaitCombat, pawn.Position);
		}
	}
}
