using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public static class KeyBindingDefGenerator
	{
		public static IEnumerable<KeyBindingCategoryDef> ImpliedKeyBindingCategoryDefs()
		{
			List<KeyBindingCategoryDef> gameUniversalCats = (from d in DefDatabase<KeyBindingCategoryDef>.AllDefs
			where d.isGameUniversal
			select d).ToList<KeyBindingCategoryDef>();
			foreach (DesignationCategoryDef def in DefDatabase<DesignationCategoryDef>.AllDefs)
			{
				KeyBindingCategoryDef catDef = new KeyBindingCategoryDef();
				catDef.defName = "Architect_" + def.defName;
				catDef.label = def.label + " tab";
				catDef.description = "Key bindings for the \"" + def.LabelCap + "\" section of the Architect menu";
				catDef.modContentPack = def.modContentPack;
				catDef.checkForConflicts.AddRange(gameUniversalCats);
				for (int i = 0; i < gameUniversalCats.Count; i++)
				{
					gameUniversalCats[i].checkForConflicts.Add(catDef);
				}
				def.bindingCatDef = catDef;
				yield return catDef;
			}
			yield break;
		}

		public static IEnumerable<KeyBindingDef> ImpliedKeyBindingDefs()
		{
			foreach (MainButtonDef mainTab in from td in DefDatabase<MainButtonDef>.AllDefs
			orderby td.order
			select td)
			{
				if (mainTab.defaultHotKey != KeyCode.None)
				{
					KeyBindingDef keyDef = new KeyBindingDef();
					keyDef.label = "Toggle " + mainTab.label + " tab";
					keyDef.defName = "MainTab_" + mainTab.defName;
					keyDef.category = KeyBindingCategoryDefOf.MainTabs;
					keyDef.defaultKeyCodeA = mainTab.defaultHotKey;
					keyDef.modContentPack = mainTab.modContentPack;
					mainTab.hotKey = keyDef;
					yield return keyDef;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <ImpliedKeyBindingCategoryDefs>c__Iterator0 : IEnumerable, IEnumerable<KeyBindingCategoryDef>, IEnumerator, IDisposable, IEnumerator<KeyBindingCategoryDef>
		{
			internal List<KeyBindingCategoryDef> <gameUniversalCats>__0;

			internal IEnumerator<DesignationCategoryDef> $locvar0;

			internal DesignationCategoryDef <def>__1;

			internal KeyBindingCategoryDef <catDef>__2;

			internal KeyBindingCategoryDef $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<KeyBindingCategoryDef, bool> <>f__am$cache0;

			[DebuggerHidden]
			public <ImpliedKeyBindingCategoryDefs>c__Iterator0()
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
					gameUniversalCats = (from d in DefDatabase<KeyBindingCategoryDef>.AllDefs
					where d.isGameUniversal
					select d).ToList<KeyBindingCategoryDef>();
					enumerator = DefDatabase<DesignationCategoryDef>.AllDefs.GetEnumerator();
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
						def = enumerator.Current;
						catDef = new KeyBindingCategoryDef();
						catDef.defName = "Architect_" + def.defName;
						catDef.label = def.label + " tab";
						catDef.description = "Key bindings for the \"" + def.LabelCap + "\" section of the Architect menu";
						catDef.modContentPack = def.modContentPack;
						catDef.checkForConflicts.AddRange(gameUniversalCats);
						for (int i = 0; i < gameUniversalCats.Count; i++)
						{
							gameUniversalCats[i].checkForConflicts.Add(catDef);
						}
						def.bindingCatDef = catDef;
						this.$current = catDef;
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

			KeyBindingCategoryDef IEnumerator<KeyBindingCategoryDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.KeyBindingCategoryDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<KeyBindingCategoryDef> IEnumerable<KeyBindingCategoryDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new KeyBindingDefGenerator.<ImpliedKeyBindingCategoryDefs>c__Iterator0();
			}

			private static bool <>m__0(KeyBindingCategoryDef d)
			{
				return d.isGameUniversal;
			}
		}

		[CompilerGenerated]
		private sealed class <ImpliedKeyBindingDefs>c__Iterator1 : IEnumerable, IEnumerable<KeyBindingDef>, IEnumerator, IDisposable, IEnumerator<KeyBindingDef>
		{
			internal IEnumerator<MainButtonDef> $locvar0;

			internal MainButtonDef <mainTab>__1;

			internal KeyBindingDef <keyDef>__2;

			internal KeyBindingDef $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<MainButtonDef, int> <>f__am$cache0;

			[DebuggerHidden]
			public <ImpliedKeyBindingDefs>c__Iterator1()
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
					enumerator = (from td in DefDatabase<MainButtonDef>.AllDefs
					orderby td.order
					select td).GetEnumerator();
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
					while (enumerator.MoveNext())
					{
						mainTab = enumerator.Current;
						if (mainTab.defaultHotKey != KeyCode.None)
						{
							keyDef = new KeyBindingDef();
							keyDef.label = "Toggle " + mainTab.label + " tab";
							keyDef.defName = "MainTab_" + mainTab.defName;
							keyDef.category = KeyBindingCategoryDefOf.MainTabs;
							keyDef.defaultKeyCodeA = mainTab.defaultHotKey;
							keyDef.modContentPack = mainTab.modContentPack;
							mainTab.hotKey = keyDef;
							this.$current = keyDef;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
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

			KeyBindingDef IEnumerator<KeyBindingDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.KeyBindingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<KeyBindingDef> IEnumerable<KeyBindingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new KeyBindingDefGenerator.<ImpliedKeyBindingDefs>c__Iterator1();
			}

			private static int <>m__0(MainButtonDef td)
			{
				return td.order;
			}
		}
	}
}
