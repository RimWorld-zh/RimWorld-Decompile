using System;
using RimWorld;

namespace Verse.AI
{
	public class JobGiver_IdleError : ThinkNode_JobGiver
	{
		private const int WaitTime = 100;

		public JobGiver_IdleError()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.ErrorOnce(pawn + " issued IdleError wait job. The behavior tree should never get here.", 532983, false);
			return new Job(JobDefOf.Wait)
			{
				expiryInterval = 100
			};
		}
	}
}
