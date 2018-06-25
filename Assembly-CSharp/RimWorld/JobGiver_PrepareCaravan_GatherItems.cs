using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobGiver_PrepareCaravan_GatherItems : ThinkNode_JobGiver
	{
		public JobGiver_PrepareCaravan_GatherItems()
		{
		}

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
					result = new Job(JobDefOf.PrepareCaravan_GatherItems, thing)
					{
						lord = lord
					};
				}
			}
			return result;
		}
	}
}
