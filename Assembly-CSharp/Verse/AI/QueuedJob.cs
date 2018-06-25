using System;

namespace Verse.AI
{
	// Token: 0x02000A55 RID: 2645
	public class QueuedJob : IExposable
	{
		// Token: 0x0400254E RID: 9550
		public Job job;

		// Token: 0x0400254F RID: 9551
		public JobTag? tag;

		// Token: 0x06003AE1 RID: 15073 RVA: 0x001F4610 File Offset: 0x001F2A10
		public QueuedJob()
		{
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x001F4619 File Offset: 0x001F2A19
		public QueuedJob(Job job, JobTag? tag)
		{
			this.job = job;
			this.tag = tag;
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x001F4630 File Offset: 0x001F2A30
		public void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.job, "job", new object[0]);
			Scribe_Values.Look<JobTag?>(ref this.tag, "tag", null, false);
		}
	}
}
