using System;

namespace Verse.AI
{
	// Token: 0x02000ABF RID: 2751
	public class ThinkNode_Subtree : ThinkNode
	{
		// Token: 0x040026A6 RID: 9894
		private ThinkTreeDef treeDef;

		// Token: 0x040026A7 RID: 9895
		[Unsaved]
		public ThinkNode subtreeNode;

		// Token: 0x06003D3D RID: 15677 RVA: 0x002057A4 File Offset: 0x00203BA4
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

		// Token: 0x06003D3E RID: 15678 RVA: 0x00205803 File Offset: 0x00203C03
		protected override void ResolveSubnodes()
		{
			this.subtreeNode = this.treeDef.thinkRoot.DeepCopy(true);
			this.subNodes.Add(this.subtreeNode);
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00205830 File Offset: 0x00203C30
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return this.subtreeNode.TryIssueJobPackage(pawn, jobParams);
		}
	}
}
