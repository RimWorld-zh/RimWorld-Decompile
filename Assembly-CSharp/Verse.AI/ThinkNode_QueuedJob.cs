namespace Verse.AI
{
	public class ThinkNode_QueuedJob : ThinkNode
	{
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			JobQueue jobQueue = pawn.jobs.jobQueue;
			while (jobQueue.Count > 0 && !jobQueue.Peek().job.CanBeginNow(pawn))
			{
				QueuedJob queuedJob = jobQueue.Dequeue();
				pawn.ClearReservationsForJob(queuedJob.job);
				if (pawn.jobs.debugLog)
				{
					pawn.jobs.DebugLogEvent("   Throwing away queued job that I cannot begin now: " + queuedJob.job);
				}
			}
			ThinkResult result;
			if (jobQueue.Count > 0)
			{
				QueuedJob queuedJob2 = jobQueue.Dequeue();
				if (pawn.jobs.debugLog)
				{
					pawn.jobs.DebugLogEvent("   Returning queued job: " + queuedJob2.job);
				}
				result = new ThinkResult(queuedJob2.job, this, queuedJob2.tag, false);
			}
			else
			{
				result = ThinkResult.NoJob;
			}
			return result;
		}
	}
}
