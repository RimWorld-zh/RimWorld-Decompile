using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC7 RID: 2759
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x040026B2 RID: 9906
		public int ticks = 50;

		// Token: 0x06003D53 RID: 15699 RVA: 0x00205D74 File Offset: 0x00204174
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x00205DA4 File Offset: 0x002041A4
		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.Wait)
			{
				expiryInterval = this.ticks
			};
		}
	}
}
