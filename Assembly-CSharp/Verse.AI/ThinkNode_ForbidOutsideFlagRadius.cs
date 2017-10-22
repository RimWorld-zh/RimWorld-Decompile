namespace Verse.AI
{
	public class ThinkNode_ForbidOutsideFlagRadius : ThinkNode_Priority
	{
		public float maxDistToSquadFlag = -1f;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ForbidOutsideFlagRadius thinkNode_ForbidOutsideFlagRadius = (ThinkNode_ForbidOutsideFlagRadius)base.DeepCopy(resolve);
			thinkNode_ForbidOutsideFlagRadius.maxDistToSquadFlag = this.maxDistToSquadFlag;
			return thinkNode_ForbidOutsideFlagRadius;
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			try
			{
				if (this.maxDistToSquadFlag > 0.0)
				{
					if (pawn.mindState.maxDistToSquadFlag > 0.0)
					{
						Log.Error("Squad flag was not reset properly; raiders may behave strangely");
					}
					pawn.mindState.maxDistToSquadFlag = this.maxDistToSquadFlag;
				}
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			finally
			{
				pawn.mindState.maxDistToSquadFlag = -1f;
			}
		}
	}
}
