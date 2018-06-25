using System;

namespace Verse.AI
{
	// Token: 0x02000AB9 RID: 2745
	public class ThinkNode_FilterPriority : ThinkNode
	{
		// Token: 0x04002697 RID: 9879
		public float minPriority = 0.5f;

		// Token: 0x06003D2C RID: 15660 RVA: 0x00205218 File Offset: 0x00203618
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_FilterPriority thinkNode_FilterPriority = (ThinkNode_FilterPriority)base.DeepCopy(resolve);
			thinkNode_FilterPriority.minPriority = this.minPriority;
			return thinkNode_FilterPriority;
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x00205248 File Offset: 0x00203648
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.subNodes[i].GetPriority(pawn) > this.minPriority)
				{
					ThinkResult result = this.subNodes[i].TryIssueJobPackage(pawn, jobParams);
					if (result.IsValid)
					{
						return result;
					}
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
