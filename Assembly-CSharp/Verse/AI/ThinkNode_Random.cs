using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000ABA RID: 2746
	public class ThinkNode_Random : ThinkNode
	{
		// Token: 0x0400269C RID: 9884
		private static List<ThinkNode> tempList = new List<ThinkNode>();

		// Token: 0x06003D32 RID: 15666 RVA: 0x002051C4 File Offset: 0x002035C4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_Random.tempList.Clear();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				ThinkNode_Random.tempList.Add(this.subNodes[i]);
			}
			ThinkNode_Random.tempList.Shuffle<ThinkNode>();
			for (int j = 0; j < ThinkNode_Random.tempList.Count; j++)
			{
				ThinkResult result = ThinkNode_Random.tempList[j].TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
