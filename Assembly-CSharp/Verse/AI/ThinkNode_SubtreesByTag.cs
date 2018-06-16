using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x02000AC1 RID: 2753
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		// Token: 0x06003D40 RID: 15680 RVA: 0x00205060 File Offset: 0x00203460
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x0020508F File Offset: 0x0020348F
		protected override void ResolveSubnodes()
		{
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00205094 File Offset: 0x00203494
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.matchedTrees == null)
			{
				this.matchedTrees = new List<ThinkTreeDef>();
				foreach (ThinkTreeDef thinkTreeDef in DefDatabase<ThinkTreeDef>.AllDefs)
				{
					if (thinkTreeDef.insertTag == this.insertTag)
					{
						this.matchedTrees.Add(thinkTreeDef);
					}
				}
				this.matchedTrees = (from tDef in this.matchedTrees
				orderby tDef.insertPriority descending
				select tDef).ToList<ThinkTreeDef>();
			}
			for (int i = 0; i < this.matchedTrees.Count; i++)
			{
				ThinkResult result = this.matchedTrees[i].thinkRoot.TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x040026A5 RID: 9893
		[NoTranslate]
		public string insertTag;

		// Token: 0x040026A6 RID: 9894
		[Unsaved]
		private List<ThinkTreeDef> matchedTrees = null;
	}
}
