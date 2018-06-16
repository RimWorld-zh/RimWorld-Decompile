using System;

namespace Verse.AI
{
	// Token: 0x02000AD0 RID: 2768
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		// Token: 0x06003D77 RID: 15735
		protected abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06003D78 RID: 15736 RVA: 0x0002FEC4 File Offset: 0x0002E2C4
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
