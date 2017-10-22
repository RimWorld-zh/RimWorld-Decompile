using System.Collections.Generic;

namespace Verse.AI
{
	public class ThinkNode_Random : ThinkNode
	{
		private static List<ThinkNode> tempList = new List<ThinkNode>();

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_Random.tempList.Clear();
			for (int i = 0; i < base.subNodes.Count; i++)
			{
				ThinkNode_Random.tempList.Add(base.subNodes[i]);
			}
			ThinkNode_Random.tempList.Shuffle();
			int num = 0;
			ThinkResult result;
			while (true)
			{
				if (num < ThinkNode_Random.tempList.Count)
				{
					ThinkResult thinkResult = ThinkNode_Random.tempList[num].TryIssueJobPackage(pawn, jobParams);
					if (thinkResult.IsValid)
					{
						result = thinkResult;
						break;
					}
					num++;
					continue;
				}
				result = ThinkResult.NoJob;
				break;
			}
			return result;
		}
	}
}
