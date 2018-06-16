using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC8 RID: 2760
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x06003D52 RID: 15698 RVA: 0x00205570 File Offset: 0x00203970
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x002055A0 File Offset: 0x002039A0
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
