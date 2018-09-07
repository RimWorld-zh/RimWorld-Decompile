using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public static class ScenarioLister
	{
		private static bool dirty = true;

		public static IEnumerable<Scenario> AllScenarios()
		{
			ScenarioLister.RecacheIfDirty();
			foreach (ScenarioDef scenDef in DefDatabase<ScenarioDef>.AllDefs)
			{
				yield return scenDef.scenario;
			}
			foreach (Scenario scen in ScenarioFiles.AllScenariosLocal)
			{
				yield return scen;
			}
			foreach (Scenario scen2 in ScenarioFiles.AllScenariosWorkshop)
			{
				yield return scen2;
			}
			yield break;
		}

		public static IEnumerable<Scenario> ScenariosInCategory(ScenarioCategory cat)
		{
			ScenarioLister.RecacheIfDirty();
			if (cat == ScenarioCategory.FromDef)
			{
				foreach (ScenarioDef scenDef in DefDatabase<ScenarioDef>.AllDefs)
				{
					yield return scenDef.scenario;
				}
			}
			else if (cat == ScenarioCategory.CustomLocal)
			{
				foreach (Scenario scen in ScenarioFiles.AllScenariosLocal)
				{
					yield return scen;
				}
			}
			else if (cat == ScenarioCategory.SteamWorkshop)
			{
				foreach (Scenario scen2 in ScenarioFiles.AllScenariosWorkshop)
				{
					yield return scen2;
				}
			}
			yield break;
		}

		public static bool ScenarioIsListedAnywhere(Scenario scen)
		{
			ScenarioLister.RecacheIfDirty();
			foreach (ScenarioDef scenarioDef in DefDatabase<ScenarioDef>.AllDefs)
			{
				if (scenarioDef.scenario == scen)
				{
					return true;
				}
			}
			foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
			{
				if (scen == scenario)
				{
					return true;
				}
			}
			return false;
		}

		public static void MarkDirty()
		{
			ScenarioLister.dirty = true;
		}

		private static void RecacheIfDirty()
		{
			if (ScenarioLister.dirty)
			{
				ScenarioLister.RecacheData();
			}
		}

		private static void RecacheData()
		{
			ScenarioLister.dirty = false;
			int num = ScenarioLister.ScenarioListHash();
			ScenarioFiles.RecacheData();
			if (ScenarioLister.ScenarioListHash() != num && !LongEventHandler.ShouldWaitForEvent)
			{
				Page_SelectScenario page_SelectScenario = Find.WindowStack.WindowOfType<Page_SelectScenario>();
				if (page_SelectScenario != null)
				{
					page_SelectScenario.Notify_ScenarioListChanged();
				}
			}
		}

		public static int ScenarioListHash()
		{
			int num = 9826121;
			foreach (Scenario scenario in ScenarioLister.AllScenarios())
			{
				num ^= 791 * scenario.GetHashCode() * 6121;
			}
			return num;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ScenarioLister()
		{
		}

		[CompilerGenerated]
		private sealed class <AllScenarios>c__Iterator0 : IEnumerable, IEnumerable<Scenario>, IEnumerator, IDisposable, IEnumerator<Scenario>
		{
			internal IEnumerator<ScenarioDef> $locvar0;

			internal ScenarioDef <scenDef>__1;

			internal IEnumerator<Scenario> $locvar1;

			internal Scenario <scen>__2;

			internal IEnumerator<Scenario> $locvar2;

			internal Scenario <scen>__3;

			internal Scenario $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllScenarios>c__Iterator0()
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
					ScenarioLister.RecacheIfDirty();
					enumerator = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_CF;
				case 3u:
					goto IL_156;
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
						scenDef = enumerator.Current;
						this.$current = scenDef.scenario;
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
				enumerator2 = ScenarioFiles.AllScenariosLocal.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_CF:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						scen = enumerator2.Current;
						this.$current = scen;
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
				enumerator3 = ScenarioFiles.AllScenariosWorkshop.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_156:
					switch (num)
					{
					}
					if (enumerator3.MoveNext())
					{
						scen2 = enumerator3.Current;
						this.$current = scen2;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			Scenario IEnumerator<Scenario>.Current
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
				case 2u:
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
					break;
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Scenario>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Scenario> IEnumerable<Scenario>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new ScenarioLister.<AllScenarios>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <ScenariosInCategory>c__Iterator1 : IEnumerable, IEnumerable<Scenario>, IEnumerator, IDisposable, IEnumerator<Scenario>
		{
			internal ScenarioCategory cat;

			internal IEnumerator<ScenarioDef> $locvar0;

			internal ScenarioDef <scenDef>__1;

			internal IEnumerator<Scenario> $locvar1;

			internal Scenario <scen>__2;

			internal IEnumerator<Scenario> $locvar2;

			internal Scenario <scen>__3;

			internal Scenario $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ScenariosInCategory>c__Iterator1()
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
					ScenarioLister.RecacheIfDirty();
					if (cat == ScenarioCategory.FromDef)
					{
						enumerator = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator();
						num = 4294967293u;
					}
					else
					{
						if (cat == ScenarioCategory.CustomLocal)
						{
							enumerator2 = ScenarioFiles.AllScenariosLocal.GetEnumerator();
							num = 4294967293u;
							goto Block_5;
						}
						if (cat == ScenarioCategory.SteamWorkshop)
						{
							enumerator3 = ScenarioFiles.AllScenariosWorkshop.GetEnumerator();
							num = 4294967293u;
							goto Block_7;
						}
						goto IL_1F8;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_EC;
				case 3u:
					goto IL_184;
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
						scenDef = enumerator.Current;
						this.$current = scenDef.scenario;
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
				goto IL_1F8;
				Block_5:
				try
				{
					IL_EC:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						scen = enumerator2.Current;
						this.$current = scen;
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
				goto IL_1F8;
				Block_7:
				try
				{
					IL_184:
					switch (num)
					{
					}
					if (enumerator3.MoveNext())
					{
						scen2 = enumerator3.Current;
						this.$current = scen2;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
				}
				IL_1F8:
				this.$PC = -1;
				return false;
			}

			Scenario IEnumerator<Scenario>.Current
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
				case 2u:
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
					break;
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Scenario>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Scenario> IEnumerable<Scenario>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ScenarioLister.<ScenariosInCategory>c__Iterator1 <ScenariosInCategory>c__Iterator = new ScenarioLister.<ScenariosInCategory>c__Iterator1();
				<ScenariosInCategory>c__Iterator.cat = cat;
				return <ScenariosInCategory>c__Iterator;
			}
		}
	}
}
