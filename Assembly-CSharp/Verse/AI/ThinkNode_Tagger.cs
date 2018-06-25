using System;

namespace Verse.AI
{
	// Token: 0x02000ABE RID: 2750
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		// Token: 0x040026A5 RID: 9893
		private JobTag tagToGive = JobTag.Misc;

		// Token: 0x06003D39 RID: 15673 RVA: 0x00205690 File Offset: 0x00203A90
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x002056C0 File Offset: 0x00203AC0
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

		// Token: 0x06003D3B RID: 15675 RVA: 0x00205738 File Offset: 0x00203B38
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
