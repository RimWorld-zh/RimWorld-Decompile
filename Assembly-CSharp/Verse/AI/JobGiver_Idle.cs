using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC4 RID: 2756
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x040026AA RID: 9898
		public int ticks = 50;

		// Token: 0x06003D4F RID: 15695 RVA: 0x00205968 File Offset: 0x00203D68
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x00205998 File Offset: 0x00203D98
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.Wait)
			{
				expiryInterval = this.ticks
			};
		}
	}
}
