using System;

namespace Verse.AI
{
	// Token: 0x02000AB9 RID: 2745
	public class ThinkNode_ForbidOutsideFlagRadius : ThinkNode_Priority
	{
		// Token: 0x04002691 RID: 9873
		public float maxDistToSquadFlag = -1f;

		// Token: 0x06003D2F RID: 15663 RVA: 0x00204FFC File Offset: 0x002033FC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ForbidOutsideFlagRadius thinkNode_ForbidOutsideFlagRadius = (ThinkNode_ForbidOutsideFlagRadius)base.DeepCopy(resolve);
			thinkNode_ForbidOutsideFlagRadius.maxDistToSquadFlag = this.maxDistToSquadFlag;
			return thinkNode_ForbidOutsideFlagRadius;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x0020502C File Offset: 0x0020342C
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
