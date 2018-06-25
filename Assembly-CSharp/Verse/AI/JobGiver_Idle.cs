using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC6 RID: 2758
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x040026AB RID: 9899
		public int ticks = 50;

		// Token: 0x06003D53 RID: 15699 RVA: 0x00205A94 File Offset: 0x00203E94
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x00205AC4 File Offset: 0x00203EC4
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.Wait)
			{
				expiryInterval = this.ticks
			};
		}
	}
}
