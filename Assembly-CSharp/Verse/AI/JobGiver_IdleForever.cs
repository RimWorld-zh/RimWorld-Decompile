using System;
using RimWorld;

namespace Verse.AI
{
	public class JobGiver_IdleForever : ThinkNode_JobGiver
	{
		public JobGiver_IdleForever()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.Wait_Downed);
		}
	}
}
