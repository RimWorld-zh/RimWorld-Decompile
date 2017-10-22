using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		public string insertTag;

		[Unsaved]
		private List<ThinkTreeDef> matchedTrees = null;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		protected override void ResolveSubnodes()
		{
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.matchedTrees == null)
			{
				this.matchedTrees = new List<ThinkTreeDef>();
				foreach (ThinkTreeDef allDef in DefDatabase<ThinkTreeDef>.AllDefs)
				{
					if (allDef.insertTag == this.insertTag)
					{
						this.matchedTrees.Add(allDef);
					}
				}
				this.matchedTrees = (from tDef in this.matchedTrees
				orderby tDef.insertPriority descending
				select tDef).ToList();
			}
			int num = 0;
			ThinkResult result;
			while (true)
			{
				if (num < this.matchedTrees.Count)
				{
					ThinkResult thinkResult = this.matchedTrees[num].thinkRoot.TryIssueJobPackage(pawn, jobParams);
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
