using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC8 RID: 2760
	public class JobGiver_IdleError : ThinkNode_JobGiver
	{
		// Token: 0x040026AC RID: 9900
		private const int WaitTime = 100;

		// Token: 0x06003D58 RID: 15704 RVA: 0x00205B24 File Offset: 0x00203F24
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
