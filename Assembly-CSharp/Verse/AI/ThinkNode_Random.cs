using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000ABD RID: 2749
	public class ThinkNode_Random : ThinkNode
	{
		// Token: 0x040026A4 RID: 9892
		private static List<ThinkNode> tempList = new List<ThinkNode>();

		// Token: 0x06003D36 RID: 15670 RVA: 0x002055D0 File Offset: 0x002039D0
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
