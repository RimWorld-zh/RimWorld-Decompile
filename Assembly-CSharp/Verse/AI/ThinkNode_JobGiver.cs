using System;

namespace Verse.AI
{
	// Token: 0x02000ACC RID: 2764
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		// Token: 0x06003D74 RID: 15732
		protected abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06003D75 RID: 15733 RVA: 0x0002FEA0 File Offset: 0x0002E2A0
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Job job = this.TryGiveJob(pawn);
			ThinkResult result;
			if (job == null)
			{
				result = ThinkResult.NoJob;
			}
			else
			{
				result = new ThinkResult(job, this, null, false);
			}
			return result;
		}
	}
}
