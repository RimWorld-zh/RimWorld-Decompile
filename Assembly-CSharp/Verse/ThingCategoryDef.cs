using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public class ThingCategoryDef : Def
	{
		public ThingCategoryDef parent;

		[NoTranslate]
		public string iconPath;

		public bool resourceReadoutRoot;

		[Unsaved]
		public TreeNode_ThingCategory treeNode;

		[Unsaved]
		public List<ThingCategoryDef> childCategories = new List<ThingCategoryDef>();

		[Unsaved]
		public List<ThingDef> childThingDefs = new List<ThingDef>();

		[Unsaved]
		public List<SpecialThingFilterDef> childSpecialFilters = new List<SpecialThingFilterDef>();

		[Unsaved]
		public Texture2D icon = BaseContent.BadTex;

		public ThingCategoryDef()
		{
		}

		public IEnumerable<ThingCategoryDef> Parents
		{
			get
			{
				if (this.parent != null)
				{
					yield return this.parent;
					foreach (ThingCategoryDef cat in this.parent.Parents)
					{
						yield return cat;
					}
				}
				yield break;
			}
		}

		public IEnumerable<ThingCategoryDef> ThisAndChildCategoryDefs
		{
			get
			{
				yield return this;
				foreach (ThingCategoryDef child in this.childCategories)
				{
					foreach (ThingCategoryDef subChild in child.ThisAndChildCategoryDefs)
					{
						yield return subChild;
					}
				}
				yield break;
			}
		}

		public IEnumerable<ThingDef> DescendantThingDefs
		{
			get
			{
				foreach (ThingCategoryDef childCatDef in this.ThisAndChildCategoryDefs)
				{
					foreach (ThingDef def in childCatDef.childThingDefs)
					{
						yield return def;
					}
				}
				yield break;
			}
		}

		public IEnumerable<SpecialThingFilterDef> DescendantSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef childCatDef in this.ThisAndChildCategoryDefs)
				{
					foreach (SpecialThingFilterDef sf in childCatDef.childSpecialFilters)
					{
						yield return sf;
					}
				}
				yield break;
			}
		}

		public IEnumerable<SpecialThingFilterDef> ParentsSpecialThingFilterDefs
		{
			get
			{
				foreach (ThingCategoryDef cat in this.Parents)
				{
					foreach (SpecialThingFilterDef filter in cat.childSpecialFilters)
					{
						yield return filter;
					}
				}
				yield break;
			}
		}

		public override void PostLoad()
		{
			this.treeNode = new TreeNode_ThingCategory(this);
			if (!this.iconPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		public static ThingCategoryDef Named(string defName)
		{
			return DefDatabase<ThingCategoryDef>.GetNamed(defName, true);
		}

		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		[CompilerGenerated]
		private void <PostLoad>m__0()
		{
			this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<ThingCategoryDef>, IEnumerator, IDisposable, IEnumerator<ThingCategoryDef>
		{
			internal IEnumerator<ThingCategoryDef> $locvar0;

			internal ThingCategoryDef <cat>__1;

			internal ThingCategoryDef $this;

			internal ThingCategoryDef $current;

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
					if (this.parent != null)
					{
						this.$current = this.parent;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_EE;
				case 1u:
					enumerator = this.parent.Parents.GetEnumerator();
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
						cat = enumerator.Current;
						this.$current = cat;
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
				IL_EE:
				this.$PC = -1;
				return false;
			}

			ThingCategoryDef IEnumerator<ThingCategoryDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.ThingCategoryDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingCategoryDef> IEnumerable<ThingCategoryDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingCategoryDef.<>c__Iterator0 <>c__Iterator = new ThingCategoryDef.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<ThingCategoryDef>, IEnumerator, IDisposable, IEnumerator<ThingCategoryDef>
		{
			internal List<ThingCategoryDef>.Enumerator $locvar0;

			internal ThingCategoryDef <child>__1;

			internal IEnumerator<ThingCategoryDef> $locvar1;

			internal ThingCategoryDef <subChild>__2;

			internal ThingCategoryDef $this;

			internal ThingCategoryDef $current;

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
					this.$current = this;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					enumerator = this.childCategories.GetEnumerator();
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
					case 2u:
						Block_5:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								subChild = enumerator2.Current;
								this.$current = subChild;
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
								if (enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						child = enumerator.Current;
						enumerator2 = child.ThisAndChildCategoryDefs.GetEnumerator();
						num = 4294967293u;
						goto Block_5;
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

			ThingCategoryDef IEnumerator<ThingCategoryDef>.Current
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
						try
						{
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
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
				return this.System.Collections.Generic.IEnumerable<Verse.ThingCategoryDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingCategoryDef> IEnumerable<ThingCategoryDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingCategoryDef.<>c__Iterator1 <>c__Iterator = new ThingCategoryDef.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator2 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal IEnumerator<ThingCategoryDef> $locvar0;

			internal ThingCategoryDef <childCatDef>__1;

			internal List<ThingDef>.Enumerator $locvar1;

			internal ThingDef <def>__2;

			internal ThingCategoryDef $this;

			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator2()
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
					enumerator = base.ThisAndChildCategoryDefs.GetEnumerator();
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
					case 1u:
						Block_4:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								def = enumerator2.Current;
								this.$current = def;
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
								((IDisposable)enumerator2).Dispose();
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						childCatDef = enumerator.Current;
						enumerator2 = childCatDef.childThingDefs.GetEnumerator();
						num = 4294967293u;
						goto Block_4;
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

			ThingDef IEnumerator<ThingDef>.Current
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
						try
						{
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
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
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingCategoryDef.<>c__Iterator2 <>c__Iterator = new ThingCategoryDef.<>c__Iterator2();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator3 : IEnumerable, IEnumerable<SpecialThingFilterDef>, IEnumerator, IDisposable, IEnumerator<SpecialThingFilterDef>
		{
			internal IEnumerator<ThingCategoryDef> $locvar0;

			internal ThingCategoryDef <childCatDef>__1;

			internal List<SpecialThingFilterDef>.Enumerator $locvar1;

			internal SpecialThingFilterDef <sf>__2;

			internal ThingCategoryDef $this;

			internal SpecialThingFilterDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator3()
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
					enumerator = base.ThisAndChildCategoryDefs.GetEnumerator();
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
					case 1u:
						Block_4:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								sf = enumerator2.Current;
								this.$current = sf;
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
								((IDisposable)enumerator2).Dispose();
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						childCatDef = enumerator.Current;
						enumerator2 = childCatDef.childSpecialFilters.GetEnumerator();
						num = 4294967293u;
						goto Block_4;
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

			SpecialThingFilterDef IEnumerator<SpecialThingFilterDef>.Current
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
						try
						{
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
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
				return this.System.Collections.Generic.IEnumerable<Verse.SpecialThingFilterDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<SpecialThingFilterDef> IEnumerable<SpecialThingFilterDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingCategoryDef.<>c__Iterator3 <>c__Iterator = new ThingCategoryDef.<>c__Iterator3();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator4 : IEnumerable, IEnumerable<SpecialThingFilterDef>, IEnumerator, IDisposable, IEnumerator<SpecialThingFilterDef>
		{
			internal IEnumerator<ThingCategoryDef> $locvar0;

			internal ThingCategoryDef <cat>__1;

			internal List<SpecialThingFilterDef>.Enumerator $locvar1;

			internal SpecialThingFilterDef <filter>__2;

			internal ThingCategoryDef $this;

			internal SpecialThingFilterDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator4()
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
					enumerator = base.Parents.GetEnumerator();
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
					case 1u:
						Block_4:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								filter = enumerator2.Current;
								this.$current = filter;
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
								((IDisposable)enumerator2).Dispose();
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						cat = enumerator.Current;
						enumerator2 = cat.childSpecialFilters.GetEnumerator();
						num = 4294967293u;
						goto Block_4;
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

			SpecialThingFilterDef IEnumerator<SpecialThingFilterDef>.Current
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
						try
						{
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
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
				return this.System.Collections.Generic.IEnumerable<Verse.SpecialThingFilterDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<SpecialThingFilterDef> IEnumerable<SpecialThingFilterDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingCategoryDef.<>c__Iterator4 <>c__Iterator = new ThingCategoryDef.<>c__Iterator4();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
