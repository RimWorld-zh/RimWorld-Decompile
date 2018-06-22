using System;

namespace Verse.AI
{
	// Token: 0x02000AAE RID: 2734
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		// Token: 0x06003D12 RID: 15634 RVA: 0x00055D70 File Offset: 0x00054170
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x00055DA0 File Offset: 0x000541A0
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result;
			if (this.Satisfied(pawn) == !this.invert)
			{
				result = base.TryIssueJobPackage(pawn, jobParams);
			}
			else
			{
				result = ThinkResult.NoJob;
			}
			return result;
		}

		// Token: 0x06003D14 RID: 15636
		protected abstract bool Satisfied(Pawn pawn);

		// Token: 0x0400268A RID: 9866
		public bool invert = false;
	}
}
