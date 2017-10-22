namespace Verse.AI
{
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		public bool invert = false;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return (this.Satisfied(pawn) != !this.invert) ? ThinkResult.NoJob : base.TryIssueJobPackage(pawn, jobParams);
		}

		protected abstract bool Satisfied(Pawn pawn);
	}
}
