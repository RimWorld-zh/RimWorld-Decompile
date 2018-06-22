using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x02000ABD RID: 2749
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		// Token: 0x06003D3D RID: 15677 RVA: 0x00205458 File Offset: 0x00203858
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00205487 File Offset: 0x00203887
		protected override void ResolveSubnodes()
		{
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x0020548C File Offset: 0x0020388C
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

		// Token: 0x040026A0 RID: 9888
		[NoTranslate]
		public string insertTag;

		// Token: 0x040026A1 RID: 9889
		[Unsaved]
		private List<ThinkTreeDef> matchedTrees = null;
	}
}
