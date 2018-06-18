using System;

namespace Verse.AI
{
	// Token: 0x02000ABF RID: 2751
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		// Token: 0x06003D3A RID: 15674 RVA: 0x00204F60 File Offset: 0x00203360
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00204F90 File Offset: 0x00203390
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

		// Token: 0x06003D3C RID: 15676 RVA: 0x00205008 File Offset: 0x00203408
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result = base.TryIssueJobPackage(pawn, jobParams);
			if (result.IsValid && result.Tag == null)
			{
				result = new ThinkResult(result.Job, result.SourceNode, new JobTag?(this.tagToGive), false);
			}
			return result;
		}

		// Token: 0x040026A2 RID: 9890
		private JobTag tagToGive = JobTag.Misc;
	}
}
