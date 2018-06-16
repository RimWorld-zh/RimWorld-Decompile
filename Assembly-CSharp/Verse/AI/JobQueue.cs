using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A57 RID: 2647
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x001F3E60 File Offset: 0x001F2260
		public int Count
		{
			get
			{
				return this.jobs.Count;
			}
		}

		// Token: 0x170008FF RID: 2303
		public QueuedJob this[int index]
		{
			get
			{
				return this.jobs[index];
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x001F3EA4 File Offset: 0x001F22A4
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

		// Token: 0x06003AE6 RID: 15078 RVA: 0x001F3EFA File Offset: 0x001F22FA
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, new object[0]);
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x001F3F14 File Offset: 0x001F2314
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x001F3F2A File Offset: 0x001F232A
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x001F3F40 File Offset: 0x001F2340
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

		// Token: 0x06003AEA RID: 15082 RVA: 0x001F3FA4 File Offset: 0x001F23A4
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

		// Token: 0x06003AEB RID: 15083 RVA: 0x001F3FEC File Offset: 0x001F23EC
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x001F4010 File Offset: 0x001F2410
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

		// Token: 0x06003AED RID: 15085 RVA: 0x001F4068 File Offset: 0x001F2468
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x001F4090 File Offset: 0x001F2490
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x04002544 RID: 9540
		private List<QueuedJob> jobs = new List<QueuedJob>();
	}
}
