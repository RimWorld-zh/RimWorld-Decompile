using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class ScenarioLister
	{
		private static bool dirty = true;

		public static IEnumerable<Scenario> AllScenarios()
		{
			ScenarioLister.RecacheIfDirty();
			using (IEnumerator<ScenarioDef> enumerator = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ScenarioDef scenDef = enumerator.Current;
					yield return scenDef.scenario;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (IEnumerator<Scenario> enumerator2 = ScenarioFiles.AllScenariosLocal.GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Scenario scen2 = enumerator2.Current;
					yield return scen2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (IEnumerator<Scenario> enumerator3 = ScenarioFiles.AllScenariosWorkshop.GetEnumerator())
			{
				if (enumerator3.MoveNext())
				{
					Scenario scen = enumerator3.Current;
					yield return scen;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_01dd:
			/*Error near IL_01de: Unexpected return in MoveNext()*/;
		}

		public static IEnumerable<Scenario> ScenariosInCategory(ScenarioCategory cat)
		{
			ScenarioLister.RecacheIfDirty();
			switch (cat)
			{
			case ScenarioCategory.FromDef:
			{
				using (IEnumerator<ScenarioDef> enumerator = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ScenarioDef scenDef = enumerator.Current;
						yield return scenDef.scenario;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				break;
			}
			case ScenarioCategory.CustomLocal:
			{
				using (IEnumerator<Scenario> enumerator2 = ScenarioFiles.AllScenariosLocal.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						Scenario scen2 = enumerator2.Current;
						yield return scen2;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				break;
			}
			case ScenarioCategory.SteamWorkshop:
			{
				using (IEnumerator<Scenario> enumerator3 = ScenarioFiles.AllScenariosWorkshop.GetEnumerator())
				{
					if (enumerator3.MoveNext())
					{
						Scenario scen = enumerator3.Current;
						yield return scen;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				break;
			}
			}
			yield break;
			IL_0211:
			/*Error near IL_0212: Unexpected return in MoveNext()*/;
		}

		public static bool ScenarioIsListedAnywhere(Scenario scen)
		{
			ScenarioLister.RecacheIfDirty();
			foreach (ScenarioDef allDef in DefDatabase<ScenarioDef>.AllDefs)
			{
				if (allDef.scenario == scen)
				{
					return true;
				}
			}
			foreach (Scenario item in ScenarioFiles.AllScenariosLocal)
			{
				if (scen == item)
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
			foreach (Scenario item in ScenarioLister.AllScenarios())
			{
				num ^= 791 * item.GetHashCode() * 6121;
			}
			return num;
		}
	}
}
