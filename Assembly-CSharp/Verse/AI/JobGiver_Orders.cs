using System;
using RimWorld;

namespace Verse.AI
{
	public class JobGiver_Orders : ThinkNode_JobGiver
	{
		public JobGiver_Orders()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Drafted)
			{
				return new Job(JobDefOf.Wait_Combat, pawn.Position);
			}
			return null;
		}
	}
}
