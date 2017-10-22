using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class ThinkNode_PrioritySorter : ThinkNode
	{
		public float minPriority = 0f;

		private static List<ThinkNode> workingNodes = new List<ThinkNode>();

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_PrioritySorter thinkNode_PrioritySorter = (ThinkNode_PrioritySorter)base.DeepCopy(resolve);
			thinkNode_PrioritySorter.minPriority = this.minPriority;
			return thinkNode_PrioritySorter;
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_PrioritySorter.workingNodes.Clear();
			int count = base.subNodes.Count;
			for (int num = 0; num < count; num++)
			{
				ThinkNode_PrioritySorter.workingNodes.Insert(Rand.Range(0, ThinkNode_PrioritySorter.workingNodes.Count - 1), base.subNodes[num]);
			}
			ThinkResult result;
			while (true)
			{
				if (ThinkNode_PrioritySorter.workingNodes.Count > 0)
				{
					float num2 = 0f;
					int num3 = -1;
					for (int i = 0; i < ThinkNode_PrioritySorter.workingNodes.Count; i++)
					{
						float num4 = 0f;
						try
						{
							num4 = ThinkNode_PrioritySorter.workingNodes[i].GetPriority(pawn);
						}
						catch (Exception ex)
						{
							Log.Error("Exception in " + base.GetType() + " GetPriority: " + ex.ToString());
						}
						if (!(num4 <= 0.0) && !(num4 < this.minPriority) && num4 > num2)
						{
							num2 = num4;
							num3 = i;
						}
					}
					if (num3 != -1)
					{
						ThinkResult thinkResult = ThinkResult.NoJob;
						try
						{
							thinkResult = ThinkNode_PrioritySorter.workingNodes[num3].TryIssueJobPackage(pawn, jobParams);
						}
						catch (Exception ex2)
						{
							Log.Error("Exception in " + base.GetType() + " TryIssueJobPackage: " + ex2.ToString());
						}
						if (thinkResult.IsValid)
						{
							result = thinkResult;
							break;
						}
						ThinkNode_PrioritySorter.workingNodes.RemoveAt(num3);
						continue;
					}
				}
				result = ThinkResult.NoJob;
				break;
			}
			return result;
		}
	}
}
