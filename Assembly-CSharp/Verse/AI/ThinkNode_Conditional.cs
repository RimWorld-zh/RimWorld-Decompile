using System;

namespace Verse.AI
{
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		public bool invert = false;

		protected ThinkNode_Conditional()
		{
		}

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

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

		protected abstract bool Satisfied(Pawn pawn);
	}
}
