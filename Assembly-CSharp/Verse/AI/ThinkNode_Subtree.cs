using System;

namespace Verse.AI
{
	// Token: 0x02000ABC RID: 2748
	public class ThinkNode_Subtree : ThinkNode
	{
		// Token: 0x0400269E RID: 9886
		private ThinkTreeDef treeDef;

		// Token: 0x0400269F RID: 9887
		[Unsaved]
		public ThinkNode subtreeNode;

		// Token: 0x06003D39 RID: 15673 RVA: 0x00205398 File Offset: 0x00203798
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

		// Token: 0x06003D3A RID: 15674 RVA: 0x002053F7 File Offset: 0x002037F7
		protected override void ResolveSubnodes()
		{
			this.subtreeNode = this.treeDef.thinkRoot.DeepCopy(true);
			this.subNodes.Add(this.subtreeNode);
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00205424 File Offset: 0x00203824
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return this.subtreeNode.TryIssueJobPackage(pawn, jobParams);
		}
	}
}
