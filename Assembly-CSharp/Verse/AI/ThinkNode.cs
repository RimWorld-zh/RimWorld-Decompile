using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

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

		protected ThinkNode()
		{
		}

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
				foreach (ThinkNode c in this.ChildrenRecursive)
				{
					yield return c;
				}
				yield break;
			}
		}

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

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<ThinkNode>, IEnumerator, IDisposable, IEnumerator<ThinkNode>
		{
			internal IEnumerator<ThinkNode> $locvar0;

			internal ThinkNode <c>__1;

			internal ThinkNode $this;

			internal ThinkNode $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					this.$current = this;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					enumerator = base.ChildrenRecursive.GetEnumerator();
					num = 4294967293u;
					break;
				case 2u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						c = enumerator.Current;
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			ThinkNode IEnumerator<ThinkNode>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.ThinkNode>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThinkNode> IEnumerable<ThinkNode>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThinkNode.<>c__Iterator0 <>c__Iterator = new ThinkNode.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<ThinkNode>, IEnumerator, IDisposable, IEnumerator<ThinkNode>
		{
			internal int <i>__1;

			internal IEnumerator<ThinkNode> $locvar0;

			internal ThinkNode <subSubNode>__2;

			internal ThinkNode $this;

			internal ThinkNode $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					Block_2:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							subSubNode = enumerator.Current;
							this.$current = subSubNode;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					i++;
					break;
				default:
					return false;
				}
				if (i < this.subNodes.Count)
				{
					enumerator = this.subNodes[i].ThisAndChildrenRecursive.GetEnumerator();
					num = 4294967293u;
					goto Block_2;
				}
				this.$PC = -1;
				return false;
			}

			ThinkNode IEnumerator<ThinkNode>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.ThinkNode>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThinkNode> IEnumerable<ThinkNode>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThinkNode.<>c__Iterator1 <>c__Iterator = new ThinkNode.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
