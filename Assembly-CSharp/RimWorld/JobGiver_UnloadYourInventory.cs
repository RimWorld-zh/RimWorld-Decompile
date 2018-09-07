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
			if (!pawn.inventory.UnloadEverything)
			{
				return null;
			}
			return new Job(JobDefOf.UnloadYourInventory);
		}
	}
}
