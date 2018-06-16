using System;

namespace Verse.AI
{
	// Token: 0x02000AB2 RID: 2738
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		// Token: 0x06003D15 RID: 15637 RVA: 0x00055D5C File Offset: 0x0005415C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x00055D8C File Offset: 0x0005418C
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

		// Token: 0x06003D17 RID: 15639
		protected abstract bool Satisfied(Pawn pawn);

		// Token: 0x0400268F RID: 9871
		public bool invert = false;
	}
}
