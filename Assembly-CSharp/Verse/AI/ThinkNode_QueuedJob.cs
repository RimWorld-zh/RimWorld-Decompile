using System;

namespace Verse.AI
{
	// Token: 0x02000ACF RID: 2767
	public class ThinkNode_QueuedJob : ThinkNode
	{
		// Token: 0x040026B9 RID: 9913
		public bool inBedOnly;

		// Token: 0x06003D7B RID: 15739 RVA: 0x00205F78 File Offset: 0x00204378
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_QueuedJob thinkNode_QueuedJob = (ThinkNode_QueuedJob)base.DeepCopy(resolve);
			thinkNode_QueuedJob.inBedOnly = this.inBedOnly;
			return thinkNode_QueuedJob;
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x00205FA8 File Offset: 0x002043A8
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			JobQueue jobQueue = pawn.jobs.jobQueue;
			bool flag = pawn.Downed || jobQueue.AnyCanBeginNow(pawn, this.inBedOnly);
			if (flag)
			{
				while (jobQueue.Count > 0 && !jobQueue.Peek().job.CanBeginNow(pawn, this.inBedOnly))
				{
					QueuedJob queuedJob = jobQueue.Dequeue();
					pawn.ClearReservationsForJob(queuedJob.job);
					if (pawn.jobs.debugLog)
					{
						pawn.jobs.DebugLogEvent("   Throwing away queued job that I cannot begin now: " + queuedJob.job);
					}
				}
			}
			ThinkResult result;
			if (jobQueue.Count > 0 && jobQueue.Peek().job.CanBeginNow(pawn, this.inBedOnly))
			{
				QueuedJob queuedJob2 = jobQueue.Dequeue();
				if (pawn.jobs.debugLog)
				{
					pawn.jobs.DebugLogEvent("   Returning queued job: " + queuedJob2.job);
				}
				result = new ThinkResult(queuedJob2.job, this, queuedJob2.tag, true);
			}
			else
			{
				result = ThinkResult.NoJob;
			}
			return result;
		}
	}
}
