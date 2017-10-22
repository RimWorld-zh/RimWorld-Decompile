using System;

namespace Verse.AI
{
	public class ThinkNode_Priority : ThinkNode
	{
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = base.subNodes.Count;
			for (int num = 0; num < count; num++)
			{
				ThinkResult result = ThinkResult.NoJob;
				try
				{
					result = base.subNodes[num].TryIssueJobPackage(pawn, jobParams);
				}
				catch (Exception ex)
				{
					Log.Error("Exception in " + base.GetType() + " TryIssueJobPackage: " + ex.ToString());
				}
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
