using System;

namespace Verse.AI
{
	// Token: 0x02000AB6 RID: 2742
	public class ThinkNode_FilterPriority : ThinkNode
	{
		// Token: 0x06003D28 RID: 15656 RVA: 0x00204E0C File Offset: 0x0020320C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_FilterPriority thinkNode_FilterPriority = (ThinkNode_FilterPriority)base.DeepCopy(resolve);
			thinkNode_FilterPriority.minPriority = this.minPriority;
			return thinkNode_FilterPriority;
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00204E3C File Offset: 0x0020323C
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

		// Token: 0x0400268F RID: 9871
		public float minPriority = 0.5f;
	}
}
