namespace Verse.AI
{
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		protected abstract Job TryGiveJob(Pawn pawn);

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			try
			{
				if (jobParams.maxDistToSquadFlag > 0.0)
				{
					if (pawn.mindState.maxDistToSquadFlag > 0.0)
					{
						Log.Error("Squad flag was not reset properly; raiders may behave strangely");
					}
					pawn.mindState.maxDistToSquadFlag = jobParams.maxDistToSquadFlag;
				}
				Job job = this.TryGiveJob(pawn);
				if (job == null)
				{
					return ThinkResult.NoJob;
				}
				return new ThinkResult(job, this, default(JobTag?));
				IL_0071:
				ThinkResult result;
				return result;
			}
			finally
			{
				pawn.mindState.maxDistToSquadFlag = -1f;
			}
		}
	}
}
