using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobGiver_PrepareCaravan_GatherItems : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				result = null;
			}
			else
			{
				Lord lord = pawn.GetLord();
				Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lord);
				if (thing == null)
				{
					result = null;
				}
				else
				{
					Job job = new Job(JobDefOf.PrepareCaravan_GatherItems, thing);
					job.lord = lord;
					result = job;
				}
			}
			return result;
		}
	}
}
