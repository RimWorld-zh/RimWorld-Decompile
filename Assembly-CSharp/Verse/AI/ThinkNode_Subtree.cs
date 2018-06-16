using System;

namespace Verse.AI
{
	// Token: 0x02000AC0 RID: 2752
	public class ThinkNode_Subtree : ThinkNode
	{
		// Token: 0x06003D3C RID: 15676 RVA: 0x00204FA0 File Offset: 0x002033A0
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

		// Token: 0x06003D3D RID: 15677 RVA: 0x00204FFF File Offset: 0x002033FF
		protected override void ResolveSubnodes()
		{
			this.subtreeNode = this.treeDef.thinkRoot.DeepCopy(true);
			this.subNodes.Add(this.subtreeNode);
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x0020502C File Offset: 0x0020342C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return this.subtreeNode.TryIssueJobPackage(pawn, jobParams);
		}

		// Token: 0x040026A3 RID: 9891
		private ThinkTreeDef treeDef;

		// Token: 0x040026A4 RID: 9892
		[Unsaved]
		public ThinkNode subtreeNode;
	}
}
