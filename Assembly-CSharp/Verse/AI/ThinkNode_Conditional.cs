using System;

namespace Verse.AI
{
	// Token: 0x02000AB1 RID: 2737
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		// Token: 0x04002692 RID: 9874
		public bool invert = false;

		// Token: 0x06003D16 RID: 15638 RVA: 0x00055D6C File Offset: 0x0005416C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x00055D9C File Offset: 0x0005419C
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

		// Token: 0x06003D18 RID: 15640
		protected abstract bool Satisfied(Pawn pawn);
	}
}
