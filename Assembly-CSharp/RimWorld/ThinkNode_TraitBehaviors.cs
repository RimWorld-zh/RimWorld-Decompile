using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001ED RID: 493
	public class ThinkNode_TraitBehaviors : ThinkNode
	{
		// Token: 0x06000995 RID: 2453 RVA: 0x00056FD4 File Offset: 0x000553D4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			List<Trait> allTraits = pawn.story.traits.allTraits;
			for (int i = 0; i < allTraits.Count; i++)
			{
				ThinkTreeDef thinkTree = allTraits[i].CurrentData.thinkTree;
				if (thinkTree != null)
				{
					return thinkTree.thinkRoot.TryIssueJobPackage(pawn, jobParams);
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
