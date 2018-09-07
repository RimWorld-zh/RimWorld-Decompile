using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public class TreeNode_ThingCategory : TreeNode
	{
		public ThingCategoryDef catDef;

		public TreeNode_ThingCategory(ThingCategoryDef def)
		{
			this.catDef = def;
		}

		public string Label
		{
			get
			{
				return this.catDef.label;
			}
		}

		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodesAndThis
		{
			get
			{
				foreach (ThingCategoryDef other in this.catDef.ThisAndChildCategoryDefs)
				{
					yield return other.treeNode;
				}
				yield break;
			}
		}

		public IEnumerable<TreeNode_ThingCategory> ChildCategoryNodes
		{
			get
			{
				foreach (ThingCategoryDef other in this.catDef.childCategories)
				{
					yield return other.treeNode;
				}
				yield break;
			}
		}

		public override string ToString()
		{
			return this.catDef.defName;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<TreeNode_ThingCategory>, IEnumerator, IDisposable, IEnumerator<TreeNode_ThingCategory>
		{
			internal IEnumerator<ThingCategoryDef> $locvar0;

			internal ThingCategoryDef <other>__1;

			internal TreeNode_ThingCategory $this;

			internal TreeNode_ThingCategory $current;

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
					enumerator = this.catDef.ThisAndChildCategoryDefs.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
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
						other = enumerator.Current;
						this.$current = other.treeNode;
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
				this.$PC = -1;
				return false;
			}

			TreeNode_ThingCategory IEnumerator<TreeNode_ThingCategory>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.TreeNode_ThingCategory>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<TreeNode_ThingCategory> IEnumerable<TreeNode_ThingCategory>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TreeNode_ThingCategory.<>c__Iterator0 <>c__Iterator = new TreeNode_ThingCategory.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<TreeNode_ThingCategory>, IEnumerator, IDisposable, IEnumerator<TreeNode_ThingCategory>
		{
			internal List<ThingCategoryDef>.Enumerator $locvar0;

			internal ThingCategoryDef <other>__1;

			internal TreeNode_ThingCategory $this;

			internal TreeNode_ThingCategory $current;

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
					enumerator = this.catDef.childCategories.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
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
						other = enumerator.Current;
						this.$current = other.treeNode;
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
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			TreeNode_ThingCategory IEnumerator<TreeNode_ThingCategory>.Current
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
						((IDisposable)enumerator).Dispose();
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
				return this.System.Collections.Generic.IEnumerable<Verse.TreeNode_ThingCategory>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<TreeNode_ThingCategory> IEnumerable<TreeNode_ThingCategory>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TreeNode_ThingCategory.<>c__Iterator1 <>c__Iterator = new TreeNode_ThingCategory.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
