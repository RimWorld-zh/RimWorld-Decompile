using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A57 RID: 2647
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003AE5 RID: 15077 RVA: 0x001F3F34 File Offset: 0x001F2334
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
		// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x001F3F78 File Offset: 0x001F2378
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

		// Token: 0x06003AE8 RID: 15080 RVA: 0x001F3FCE File Offset: 0x001F23CE
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, new object[0]);
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x001F3FE8 File Offset: 0x001F23E8
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x001F3FFE File Offset: 0x001F23FE
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x001F4014 File Offset: 0x001F2414
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

		// Token: 0x06003AEC RID: 15084 RVA: 0x001F4078 File Offset: 0x001F2478
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

		// Token: 0x06003AED RID: 15085 RVA: 0x001F40C0 File Offset: 0x001F24C0
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x001F40E4 File Offset: 0x001F24E4
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

		// Token: 0x06003AEF RID: 15087 RVA: 0x001F413C File Offset: 0x001F253C
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x001F4164 File Offset: 0x001F2564
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x04002544 RID: 9540
		private List<QueuedJob> jobs = new List<QueuedJob>();
	}
}
