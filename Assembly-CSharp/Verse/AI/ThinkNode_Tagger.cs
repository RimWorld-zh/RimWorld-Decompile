using System;

namespace Verse.AI
{
	// Token: 0x02000ABB RID: 2747
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		// Token: 0x06003D35 RID: 15669 RVA: 0x00205284 File Offset: 0x00203684
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x002052B4 File Offset: 0x002036B4
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

		// Token: 0x06003D37 RID: 15671 RVA: 0x0020532C File Offset: 0x0020372C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result = base.TryIssueJobPackage(pawn, jobParams);
			if (result.IsValid && result.Tag == null)
			{
				result = new ThinkResult(result.Job, result.SourceNode, new JobTag?(this.tagToGive), false);
			}
			return result;
		}

		// Token: 0x0400269D RID: 9885
		private JobTag tagToGive = JobTag.Misc;
	}
}
