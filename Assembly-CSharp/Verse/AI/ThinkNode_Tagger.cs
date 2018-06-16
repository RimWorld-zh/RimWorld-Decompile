using System;

namespace Verse.AI
{
	// Token: 0x02000ABF RID: 2751
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		// Token: 0x06003D38 RID: 15672 RVA: 0x00204E8C File Offset: 0x0020328C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00204EBC File Offset: 0x002032BC
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

		// Token: 0x06003D3A RID: 15674 RVA: 0x00204F34 File Offset: 0x00203334
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
