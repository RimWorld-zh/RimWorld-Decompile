using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC8 RID: 2760
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x06003D54 RID: 15700 RVA: 0x00205644 File Offset: 0x00203A44
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x00205674 File Offset: 0x00203A74
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.Wait)
			{
				expiryInterval = this.ticks
			};
		}

		// Token: 0x040026AF RID: 9903
		public int ticks = 50;
	}
}
