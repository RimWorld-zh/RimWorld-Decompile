using System;

namespace Verse.AI
{
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		private JobTag tagToGive = JobTag.Misc;

		public ThinkNode_Tagger()
		{
		}

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		public override float GetPriority(Pawn pawn)
		{
			float result;
			if (this.priority >= 0f)
			{
				result = this.priority;
			}
			else if (this.subNodes.Any<ThinkNode>())
			{
				result = this.subNodes[0].GetPriority(pawn);
			}
			else
			{
				Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode(), false);
				result = 0f;
			}
			return result;
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result = base.TryIssueJobPackage(pawn, jobParams);
			if (result.IsValid && result.Tag == null)
			{
				result = new ThinkResult(result.Job, result.SourceNode, new JobTag?(this.tagToGive), false);
			}
			return result;
		}
	}
}
