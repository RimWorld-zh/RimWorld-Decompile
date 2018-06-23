using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A53 RID: 2643
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		// Token: 0x0400253F RID: 9535
		private List<QueuedJob> jobs = new List<QueuedJob>();

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x001F422C File Offset: 0x001F262C
		public int Count
		{
			get
			{
				return this.jobs.Count;
			}
		}

		// Token: 0x17000900 RID: 2304
		public QueuedJob this[int index]
		{
			get
			{
				return this.jobs[index];
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x001F4270 File Offset: 0x001F2670
		public bool AnyPlayerForced
		{
			get
			{
				for (int i = 0; i < this.jobs.Count; i++)
				{
					if (this.jobs[i].job.playerForced)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x001F42C6 File Offset: 0x001F26C6
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, new object[0]);
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x001F42E0 File Offset: 0x001F26E0
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x001F42F6 File Offset: 0x001F26F6
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x001F430C File Offset: 0x001F270C
		public QueuedJob Extract(Job j)
		{
			int num = this.jobs.FindIndex((QueuedJob qj) => qj.job == j);
			QueuedJob result;
			if (num >= 0)
			{
				QueuedJob queuedJob = this.jobs[num];
				this.jobs.RemoveAt(num);
				result = queuedJob;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x001F4370 File Offset: 0x001F2770
		public QueuedJob Dequeue()
		{
			QueuedJob result;
			if (this.jobs.NullOrEmpty<QueuedJob>())
			{
				result = null;
			}
			else
			{
				QueuedJob queuedJob = this.jobs[0];
				this.jobs.RemoveAt(0);
				result = queuedJob;
			}
			return result;
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x001F43B8 File Offset: 0x001F27B8
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x001F43DC File Offset: 0x001F27DC
		public bool AnyCanBeginNow(Pawn pawn, bool whileLyingDown)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				if (this.jobs[i].job.CanBeginNow(pawn, whileLyingDown))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x001F4434 File Offset: 0x001F2834
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x001F445C File Offset: 0x001F285C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}
	}
}
