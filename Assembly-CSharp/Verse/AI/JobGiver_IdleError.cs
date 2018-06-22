using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC6 RID: 2758
	public class JobGiver_IdleError : ThinkNode_JobGiver
	{
		// Token: 0x06003D54 RID: 15700 RVA: 0x002059F8 File Offset: 0x00203DF8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Log.ErrorOnce(pawn + " issued IdleError wait job. The behavior tree should never get here.", 532983, false);
			return new Job(JobDefOf.Wait)
			{
				expiryInterval = 100
			};
		}

		// Token: 0x040026AB RID: 9899
		private const int WaitTime = 100;
	}
}
