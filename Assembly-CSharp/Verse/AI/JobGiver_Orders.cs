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
			Job result;
			if (pawn.Drafted)
			{
				result = new Job(JobDefOf.Wait_Combat, pawn.Position);
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
