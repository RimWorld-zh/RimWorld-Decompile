using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000ACF RID: 2767
	public abstract class ThinkNode
	{
		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06003D6B RID: 15723 RVA: 0x0002F8A8 File Offset: 0x0002DCA8
		public int UniqueSaveKey
		{
			get
			{
				return this.uniqueSaveKeyInt;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06003D6C RID: 15724 RVA: 0x0002F8C4 File Offset: 0x0002DCC4
		public IEnumerable<ThinkNode> ThisAndChildrenRecursive
		{
			get
			{
				yield return this;
				foreach (ThinkNode c in this.ChildrenRecursive)
				{
					yield return c;
				}
				yield break;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06003D6D RID: 15725 RVA: 0x0002F8F0 File Offset: 0x0002DCF0
		public IEnumerable<ThinkNode> ChildrenRecursive
		{
			get
			{
				for (int i = 0; i < this.subNodes.Count; i++)
				{
					foreach (ThinkNode subSubNode in this.subNodes[i].ThisAndChildrenRecursive)
					{
						yield return subSubNode;
					}
				}
				yield break;
			}
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x0002F91C File Offset: 0x0002DD1C
		public virtual float GetPriority(Pawn pawn)
		{
			float result;
			if (this.priority < 0f)
			{
				Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode(), false);
				result = 0f;
			}
			else
			{
				result = this.priority;
			}
			return result;
		}

		// Token: 0x06003D6F RID: 15727
		public abstract ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams);

		// Token: 0x06003D70 RID: 15728 RVA: 0x0002F96A File Offset: 0x0002DD6A
		protected virtual void ResolveSubnodes()
		{
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x0002F970 File Offset: 0x0002DD70
		public void ResolveSubnodesAndRecur()
		{
			if (this.uniqueSaveKeyInt == -2)
			{
				this.ResolveSubnodes();
				for (int i = 0; i < this.subNodes.Count; i++)
				{
					this.subNodes[i].ResolveSubnodesAndRecur();
				}
			}
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x0002F9C6 File Offset: 0x0002DDC6
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x0002F9CC File Offset: 0x0002DDCC
		public virtual ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode thinkNode = (ThinkNode)Activator.CreateInstance(base.GetType());
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				thinkNode.subNodes.Add(this.subNodes[i].DeepCopy(resolve));
			}
			thinkNode.priority = this.priority;
			thinkNode.leaveJoinableLordIfIssuesJob = this.leaveJoinableLordIfIssuesJob;
			thinkNode.uniqueSaveKeyInt = this.uniqueSaveKeyInt;
			if (resolve)
			{
				thinkNode.ResolveSubnodesAndRecur();
			}
			ThinkTreeKeyAssigner.AssignSingleKey(thinkNode, 0);
			return thinkNode;
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x0002FA67 File Offset: 0x0002DE67
		internal void SetUniqueSaveKey(int key)
		{
			this.uniqueSaveKeyInt = key;
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x0002FA74 File Offset: 0x0002DE74
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.uniqueSaveKeyInt, 1157295731);
		}

		// Token: 0x040026B6 RID: 9910
		public List<ThinkNode> subNodes = new List<ThinkNode>();

		// Token: 0x040026B7 RID: 9911
		public bool leaveJoinableLordIfIssuesJob;

		// Token: 0x040026B8 RID: 9912
		protected float priority = -1f;

		// Token: 0x040026B9 RID: 9913
		[Unsaved]
		private int uniqueSaveKeyInt = -2;

		// Token: 0x040026BA RID: 9914
		[Unsaved]
		public ThinkNode parent;

		// Token: 0x040026BB RID: 9915
		public const int InvalidSaveKey = -1;

		// Token: 0x040026BC RID: 9916
		protected const int UnresolvedSaveKey = -2;
	}
}
