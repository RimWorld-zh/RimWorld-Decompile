using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_UnloadYourInventory : ThinkNode_JobGiver
	{
		public JobGiver_UnloadYourInventory()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.inventory.UnloadEverything)
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.UnloadYourInventory);
			}
			return result;
		}
	}
}
