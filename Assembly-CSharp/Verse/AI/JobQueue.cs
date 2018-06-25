using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A55 RID: 2645
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		// Token: 0x04002540 RID: 9536
		private List<QueuedJob> jobs = new List<QueuedJob>();

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003AE4 RID: 15076 RVA: 0x001F4358 File Offset: 0x001F2758
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
		// (get) Token: 0x06003AE6 RID: 15078 RVA: 0x001F439C File Offset: 0x001F279C
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

		// Token: 0x06003AE7 RID: 15079 RVA: 0x001F43F2 File Offset: 0x001F27F2
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, new object[0]);
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x001F440C File Offset: 0x001F280C
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x001F4422 File Offset: 0x001F2822
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x001F4438 File Offset: 0x001F2838
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

		// Token: 0x06003AEB RID: 15083 RVA: 0x001F449C File Offset: 0x001F289C
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

		// Token: 0x06003AEC RID: 15084 RVA: 0x001F44E4 File Offset: 0x001F28E4
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x001F4508 File Offset: 0x001F2908
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

		// Token: 0x06003AEE RID: 15086 RVA: 0x001F4560 File Offset: 0x001F2960
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x001F4588 File Offset: 0x001F2988
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}
	}
}
