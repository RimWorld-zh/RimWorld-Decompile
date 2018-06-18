using System;

namespace Verse.AI
{
	// Token: 0x02000ABA RID: 2746
	public class ThinkNode_FilterPriority : ThinkNode
	{
		// Token: 0x06003D2D RID: 15661 RVA: 0x00204AE8 File Offset: 0x00202EE8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_FilterPriority thinkNode_FilterPriority = (ThinkNode_FilterPriority)base.DeepCopy(resolve);
			thinkNode_FilterPriority.minPriority = this.minPriority;
			return thinkNode_FilterPriority;
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00204B18 File Offset: 0x00202F18
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

		// Token: 0x04002694 RID: 9876
		public float minPriority = 0.5f;
	}
}
