using System;

namespace Verse.AI
{
	// Token: 0x02000ABA RID: 2746
	public class ThinkNode_ForbidOutsideFlagRadius : ThinkNode_Priority
	{
		// Token: 0x04002698 RID: 9880
		public float maxDistToSquadFlag = -1f;

		// Token: 0x06003D2F RID: 15663 RVA: 0x002052DC File Offset: 0x002036DC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ForbidOutsideFlagRadius thinkNode_ForbidOutsideFlagRadius = (ThinkNode_ForbidOutsideFlagRadius)base.DeepCopy(resolve);
			thinkNode_ForbidOutsideFlagRadius.maxDistToSquadFlag = this.maxDistToSquadFlag;
			return thinkNode_ForbidOutsideFlagRadius;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x0020530C File Offset: 0x0020370C
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
	}
}
