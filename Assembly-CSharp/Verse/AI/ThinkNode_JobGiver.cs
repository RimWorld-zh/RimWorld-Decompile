using System;

namespace Verse.AI
{
	// Token: 0x02000ACE RID: 2766
	public abstract class ThinkNode_JobGiver : ThinkNode
	{
		// Token: 0x06003D78 RID: 15736
		protected abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06003D79 RID: 15737 RVA: 0x0002FEA0 File Offset: 0x0002E2A0
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
