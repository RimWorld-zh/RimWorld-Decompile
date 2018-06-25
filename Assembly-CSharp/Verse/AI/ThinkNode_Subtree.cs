using System;

namespace Verse.AI
{
	// Token: 0x02000ABE RID: 2750
	public class ThinkNode_Subtree : ThinkNode
	{
		// Token: 0x0400269F RID: 9887
		private ThinkTreeDef treeDef;

		// Token: 0x040026A0 RID: 9888
		[Unsaved]
		public ThinkNode subtreeNode;

		// Token: 0x06003D3D RID: 15677 RVA: 0x002054C4 File Offset: 0x002038C4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Subtree thinkNode_Subtree = (ThinkNode_Subtree)base.DeepCopy(false);
			thinkNode_Subtree.treeDef = this.treeDef;
			if (resolve)
			{
				thinkNode_Subtree.ResolveSubnodesAndRecur();
				thinkNode_Subtree.subtreeNode = thinkNode_Subtree.subNodes[this.subNodes.IndexOf(this.subtreeNode)];
			}
			return thinkNode_Subtree;
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00205523 File Offset: 0x00203923
		protected override void ResolveSubnodes()
		{
			this.subtreeNode = this.treeDef.thinkRoot.DeepCopy(true);
			this.subNodes.Add(this.subtreeNode);
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00205550 File Offset: 0x00203950
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return this.subtreeNode.TryIssueJobPackage(pawn, jobParams);
		}
	}
}
