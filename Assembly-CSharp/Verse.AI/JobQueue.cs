using System;
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
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.jobs.Count)
					{
						if (this.jobs[num].job.playerForced)
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
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
			int num = this.jobs.FindIndex((Predicate<QueuedJob>)((QueuedJob qj) => qj.job == j));
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
			if (this.jobs.NullOrEmpty())
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
