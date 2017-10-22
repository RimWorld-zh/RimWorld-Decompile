using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_UnloadYourInventory : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			return pawn.inventory.UnloadEverything ? new Job(JobDefOf.UnloadYourInventory) : null;
		}
	}
}
