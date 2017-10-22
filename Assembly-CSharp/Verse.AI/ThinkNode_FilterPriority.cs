namespace Verse.AI
{
	public class ThinkNode_FilterPriority : ThinkNode
	{
		public float minPriority = 0.5f;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_FilterPriority thinkNode_FilterPriority = (ThinkNode_FilterPriority)base.DeepCopy(resolve);
			thinkNode_FilterPriority.minPriority = this.minPriority;
			return thinkNode_FilterPriority;
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = base.subNodes.Count;
			int num = 0;
			ThinkResult result;
			while (true)
			{
				if (num < count)
				{
					if (base.subNodes[num].GetPriority(pawn) > this.minPriority)
					{
						ThinkResult thinkResult = base.subNodes[num].TryIssueJobPackage(pawn, jobParams);
						if (thinkResult.IsValid)
						{
							result = thinkResult;
							break;
						}
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
