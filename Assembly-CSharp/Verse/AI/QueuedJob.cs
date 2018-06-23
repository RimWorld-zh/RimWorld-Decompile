using System;

namespace Verse.AI
{
	// Token: 0x02000A52 RID: 2642
	public class QueuedJob : IExposable
	{
		// Token: 0x0400253D RID: 9533
		public Job job;

		// Token: 0x0400253E RID: 9534
		public JobTag? tag;

		// Token: 0x06003ADC RID: 15068 RVA: 0x001F41B8 File Offset: 0x001F25B8
		public QueuedJob()
		{
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x001F41C1 File Offset: 0x001F25C1
		public QueuedJob(Job job, JobTag? tag)
		{
			this.job = job;
			this.tag = tag;
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x001F41D8 File Offset: 0x001F25D8
		public void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.job, "job", new object[0]);
			Scribe_Values.Look<JobTag?>(ref this.tag, "tag", null, false);
		}
	}
}
