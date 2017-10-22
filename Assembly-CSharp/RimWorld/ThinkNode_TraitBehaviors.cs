using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_TraitBehaviors : ThinkNode
	{
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			List<Trait> allTraits = pawn.story.traits.allTraits;
			int num = 0;
			ThinkResult result;
			while (true)
			{
				if (num < allTraits.Count)
				{
					ThinkTreeDef thinkTree = allTraits[num].CurrentData.thinkTree;
					if (thinkTree != null)
					{
						result = thinkTree.thinkRoot.TryIssueJobPackage(pawn, jobParams);
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
