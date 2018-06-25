using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x02000AC0 RID: 2752
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		// Token: 0x040026A8 RID: 9896
		[NoTranslate]
		public string insertTag;

		// Token: 0x040026A9 RID: 9897
		[Unsaved]
		private List<ThinkTreeDef> matchedTrees = null;

		// Token: 0x06003D41 RID: 15681 RVA: 0x00205864 File Offset: 0x00203C64
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00205893 File Offset: 0x00203C93
		protected override void ResolveSubnodes()
		{
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x00205898 File Offset: 0x00203C98
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
