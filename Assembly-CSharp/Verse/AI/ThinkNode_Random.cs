using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000ABC RID: 2748
	public class ThinkNode_Random : ThinkNode
	{
		// Token: 0x0400269D RID: 9885
		private static List<ThinkNode> tempList = new List<ThinkNode>();

		// Token: 0x06003D36 RID: 15670 RVA: 0x002052F0 File Offset: 0x002036F0
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
