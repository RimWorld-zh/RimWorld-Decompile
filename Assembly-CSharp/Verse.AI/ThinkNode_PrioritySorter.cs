using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class ThinkNode_PrioritySorter : ThinkNode
	{
		public float minPriority;

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
			for (int i = 0; i < count; i++)
			{
				ThinkNode_PrioritySorter.workingNodes.Insert(Rand.Range(0, ThinkNode_PrioritySorter.workingNodes.Count - 1), base.subNodes[i]);
			}
			while (ThinkNode_PrioritySorter.workingNodes.Count > 0)
			{
				float num = 0f;
				int num2 = -1;
				for (int j = 0; j < ThinkNode_PrioritySorter.workingNodes.Count; j++)
				{
					float num3 = 0f;
					try
					{
						num3 = ThinkNode_PrioritySorter.workingNodes[j].GetPriority(pawn);
					}
					catch (Exception ex)
					{
						Log.Error("Exception in " + base.GetType() + " GetPriority: " + ex.ToString());
					}
					if (!(num3 <= 0.0) && !(num3 < this.minPriority) && num3 > num)
					{
						num = num3;
						num2 = j;
					}
				}
				if (num2 != -1)
				{
					ThinkResult result = ThinkResult.NoJob;
					try
					{
						result = ThinkNode_PrioritySorter.workingNodes[num2].TryIssueJobPackage(pawn, jobParams);
					}
					catch (Exception ex2)
					{
						Log.Error("Exception in " + base.GetType() + " TryIssueJobPackage: " + ex2.ToString());
					}
					if (result.IsValid)
					{
						return result;
					}
					ThinkNode_PrioritySorter.workingNodes.RemoveAt(num2);
					continue;
				}
				break;
			}
			return ThinkResult.NoJob;
		}
	}
}
