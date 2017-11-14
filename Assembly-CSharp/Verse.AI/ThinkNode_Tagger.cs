namespace Verse.AI
{
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		private JobTag tagToGive;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		public override float GetPriority(Pawn pawn)
		{
			if (base.priority >= 0.0)
			{
				return base.priority;
			}
			if (base.subNodes.Any())
			{
				return base.subNodes[0].GetPriority(pawn);
			}
			Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode());
			return 0f;
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result = base.TryIssueJobPackage(pawn, jobParams);
			if (result.IsValid && !result.Tag.HasValue)
			{
				result = new ThinkResult(result.Job, result.SourceNode, this.tagToGive, false);
			}
			return result;
		}
	}
}
