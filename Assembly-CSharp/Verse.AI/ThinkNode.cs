using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public abstract class ThinkNode
	{
		public const int InvalidSaveKey = -1;

		protected const int UnresolvedSaveKey = -2;

		public List<ThinkNode> subNodes = new List<ThinkNode>();

		public bool leaveJoinableLordIfIssuesJob;

		protected float priority = -1f;

		[Unsaved]
		private int uniqueSaveKeyInt = -2;

		[Unsaved]
		public ThinkNode parent;

		public int UniqueSaveKey
		{
			get
			{
				return this.uniqueSaveKeyInt;
			}
		}

		public IEnumerable<ThinkNode> ThisAndChildrenRecursive
		{
			get
			{
				ThinkNode.<>c__Iterator55 <>c__Iterator = new ThinkNode.<>c__Iterator55();
				<>c__Iterator.<>f__this = this;
				ThinkNode.<>c__Iterator55 expr_0E = <>c__Iterator;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public IEnumerable<ThinkNode> ChildrenRecursive
		{
			get
			{
				ThinkNode.<>c__Iterator56 <>c__Iterator = new ThinkNode.<>c__Iterator56();
				<>c__Iterator.<>f__this = this;
				ThinkNode.<>c__Iterator56 expr_0E = <>c__Iterator;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public virtual float GetPriority(Pawn pawn)
		{
			if (this.priority < 0f)
			{
				Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + this, this.GetHashCode());
				return 0f;
			}
			return this.priority;
		}

		public abstract ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams);

		protected virtual void ResolveSubnodes()
		{
		}

		public void ResolveSubnodesAndRecur()
		{
			if (this.uniqueSaveKeyInt != -2)
			{
				return;
			}
			this.ResolveSubnodes();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				this.subNodes[i].ResolveSubnodesAndRecur();
			}
		}

		public virtual void ResolveReferences()
		{
		}

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

		internal void SetUniqueSaveKey(int key)
		{
			this.uniqueSaveKeyInt = key;
		}

		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.uniqueSaveKeyInt, 1157295731);
		}
	}
}
