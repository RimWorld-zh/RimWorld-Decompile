using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		private List<QueuedJob> jobs = new List<QueuedJob>();

		public JobQueue()
		{
		}

		public int Count
		{
			get
			{
				return this.jobs.Count;
			}
		}

		public QueuedJob this[int index]
		{
			get
			{
				return this.jobs[index];
			}
		}

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

		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, new object[0]);
		}

		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

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

		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

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

		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		[CompilerGenerated]
		private sealed class <Extract>c__AnonStorey0
		{
			internal Job j;

			public <Extract>c__AnonStorey0()
			{
			}

			internal bool <>m__0(QueuedJob qj)
			{
				return qj.job == this.j;
			}
		}
	}
}
