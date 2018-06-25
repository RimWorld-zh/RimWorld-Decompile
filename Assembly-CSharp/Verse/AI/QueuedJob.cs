using System;

namespace Verse.AI
{
	// Token: 0x02000A54 RID: 2644
	public class QueuedJob : IExposable
	{
		// Token: 0x0400253E RID: 9534
		public Job job;

		// Token: 0x0400253F RID: 9535
		public JobTag? tag;

		// Token: 0x06003AE0 RID: 15072 RVA: 0x001F42E4 File Offset: 0x001F26E4
		public QueuedJob()
		{
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x001F42ED File Offset: 0x001F26ED
		public QueuedJob(Job job, JobTag? tag)
		{
			this.job = job;
			this.tag = tag;
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x001F4304 File Offset: 0x001F2704
		public void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.job, "job", new object[0]);
			Scribe_Values.Look<JobTag?>(ref this.tag, "tag", null, false);
		}
	}
}
