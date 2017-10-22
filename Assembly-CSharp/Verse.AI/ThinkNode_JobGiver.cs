namespace Verse.AI
{
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		protected abstract Job TryGiveJob(Pawn pawn);

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Job job = this.TryGiveJob(pawn);
			return (job != null) ? new ThinkResult(job, this, default(JobTag?), false) : ThinkResult.NoJob;
		}
	}
}
