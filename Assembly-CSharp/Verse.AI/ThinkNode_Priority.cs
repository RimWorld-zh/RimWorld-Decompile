using System;

namespace Verse.AI
{
	public class ThinkNode_Priority : ThinkNode
	{
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = base.subNodes.Count;
			int num = 0;
			ThinkResult result;
			while (true)
			{
				if (num < count)
				{
					ThinkResult thinkResult = ThinkResult.NoJob;
					try
					{
						thinkResult = base.subNodes[num].TryIssueJobPackage(pawn, jobParams);
					}
					catch (Exception ex)
					{
						Log.Error("Exception in " + base.GetType() + " TryIssueJobPackage: " + ex.ToString());
					}
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
