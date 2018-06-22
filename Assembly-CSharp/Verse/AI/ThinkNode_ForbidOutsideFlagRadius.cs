using System;

namespace Verse.AI
{
	// Token: 0x02000AB7 RID: 2743
	public class ThinkNode_ForbidOutsideFlagRadius : ThinkNode_Priority
	{
		// Token: 0x06003D2B RID: 15659 RVA: 0x00204ED0 File Offset: 0x002032D0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ForbidOutsideFlagRadius thinkNode_ForbidOutsideFlagRadius = (ThinkNode_ForbidOutsideFlagRadius)base.DeepCopy(resolve);
			thinkNode_ForbidOutsideFlagRadius.maxDistToSquadFlag = this.maxDistToSquadFlag;
			return thinkNode_ForbidOutsideFlagRadius;
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00204F00 File Offset: 0x00203300
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result;
			try
			{
				if (this.maxDistToSquadFlag > 0f)
				{
					if (pawn.mindState.maxDistToSquadFlag > 0f)
					{
						Log.Error("Squad flag was not reset properly; raiders may behave strangely", false);
					}
					pawn.mindState.maxDistToSquadFlag = this.maxDistToSquadFlag;
				}
				result = base.TryIssueJobPackage(pawn, jobParams);
			}
			finally
			{
				pawn.mindState.maxDistToSquadFlag = -1f;
			}
			return result;
		}

		// Token: 0x04002690 RID: 9872
		public float maxDistToSquadFlag = -1f;
	}
}
