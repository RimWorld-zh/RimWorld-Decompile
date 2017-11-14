using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public abstract class ThinkNode
	{
		public List<ThinkNode> subNodes = new List<ThinkNode>();

		public bool leaveJoinableLordIfIssuesJob;

		protected float priority = -1f;

		[Unsaved]
		private int uniqueSaveKeyInt = -2;

		[Unsaved]
		public ThinkNode parent;

		public const int InvalidSaveKey = -1;

		protected const int UnresolvedSaveKey = -2;

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
				yield return this;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public IEnumerable<ThinkNode> ChildrenRecursive
		{
			get
			{
				for (int i = 0; i < this.subNodes.Count; i++)
				{
					using (IEnumerator<ThinkNode> enumerator = this.subNodes[i].ThisAndChildrenRecursive.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							ThinkNode subSubNode = enumerator.Current;
							yield return subSubNode;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_00fe:
				/*Error near IL_00ff: Unexpected return in MoveNext()*/;
			}
		}

		public virtual float GetPriority(Pawn pawn)
		{
			if (this.priority < 0.0)
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
			if (this.uniqueSaveKeyInt == -2)
			{
				this.ResolveSubnodes();
				for (int i = 0; i < this.subNodes.Count; i++)
				{
					this.subNodes[i].ResolveSubnodesAndRecur();
				}
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
