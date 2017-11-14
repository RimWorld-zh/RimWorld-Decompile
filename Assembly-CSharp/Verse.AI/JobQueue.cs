using System.Collections;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		private List<QueuedJob> jobs = new List<QueuedJob>();

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

		public void EnqueueFirst(Job j, JobTag? tag = default(JobTag?))
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		public void EnqueueLast(Job j, JobTag? tag = default(JobTag?))
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		public QueuedJob Extract(Job j)
		{
			int num = this.jobs.FindIndex((QueuedJob qj) => qj.job == j);
			if (num >= 0)
			{
				QueuedJob result = this.jobs[num];
				this.jobs.RemoveAt(num);
				return result;
			}
			return null;
		}

		public QueuedJob Dequeue()
		{
			if (this.jobs.NullOrEmpty())
			{
				return null;
			}
			QueuedJob result = this.jobs[0];
			this.jobs.RemoveAt(0);
			return result;
		}

		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return (IEnumerator<QueuedJob>)(object)this.jobs.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)(object)this.jobs.GetEnumerator();
		}
	}
}
