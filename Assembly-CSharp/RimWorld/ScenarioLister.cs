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
			foreach (ScenarioDef allDef in DefDatabase<ScenarioDef>.AllDefs)
			{
				yield return allDef.scenario;
			}
			foreach (Scenario item in ScenarioFiles.AllScenariosLocal)
			{
				yield return item;
			}
			foreach (Scenario item2 in ScenarioFiles.AllScenariosWorkshop)
			{
				yield return item2;
			}
		}

		public static IEnumerable<Scenario> ScenariosInCategory(ScenarioCategory cat)
		{
			ScenarioLister.RecacheIfDirty();
			switch (cat)
			{
			case ScenarioCategory.FromDef:
			{
				foreach (ScenarioDef allDef in DefDatabase<ScenarioDef>.AllDefs)
				{
					yield return allDef.scenario;
				}
				break;
			}
			case ScenarioCategory.CustomLocal:
			{
				foreach (Scenario item in ScenarioFiles.AllScenariosLocal)
				{
					yield return item;
				}
				break;
			}
			case ScenarioCategory.SteamWorkshop:
			{
				foreach (Scenario item2 in ScenarioFiles.AllScenariosWorkshop)
				{
					yield return item2;
				}
				break;
			}
			}
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
