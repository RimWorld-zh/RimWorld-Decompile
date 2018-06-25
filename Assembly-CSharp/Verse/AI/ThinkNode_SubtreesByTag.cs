using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x02000ABF RID: 2751
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		// Token: 0x040026A1 RID: 9889
		[NoTranslate]
		public string insertTag;

		// Token: 0x040026A2 RID: 9890
		[Unsaved]
		private List<ThinkTreeDef> matchedTrees = null;

		// Token: 0x06003D41 RID: 15681 RVA: 0x00205584 File Offset: 0x00203984
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x002055B3 File Offset: 0x002039B3
		protected override void ResolveSubnodes()
		{
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x002055B8 File Offset: 0x002039B8
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
	}
}
