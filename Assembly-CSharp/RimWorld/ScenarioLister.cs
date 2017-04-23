using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public static class ScenarioLister
	{
		private static bool dirty = true;

		[DebuggerHidden]
		public static IEnumerable<Scenario> AllScenarios()
		{
			ScenarioLister.<AllScenarios>c__Iterator125 <AllScenarios>c__Iterator = new ScenarioLister.<AllScenarios>c__Iterator125();
			ScenarioLister.<AllScenarios>c__Iterator125 expr_07 = <AllScenarios>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}

		[DebuggerHidden]
		public static IEnumerable<Scenario> ScenariosInCategory(ScenarioCategory cat)
		{
			ScenarioLister.<ScenariosInCategory>c__Iterator126 <ScenariosInCategory>c__Iterator = new ScenarioLister.<ScenariosInCategory>c__Iterator126();
			<ScenariosInCategory>c__Iterator.cat = cat;
			<ScenariosInCategory>c__Iterator.<$>cat = cat;
			ScenarioLister.<ScenariosInCategory>c__Iterator126 expr_15 = <ScenariosInCategory>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		public static bool ScenarioIsListedAnywhere(Scenario scen)
		{
			ScenarioLister.RecacheIfDirty();
			foreach (ScenarioDef current in DefDatabase<ScenarioDef>.AllDefs)
			{
				if (current.scenario == scen)
				{
					bool result = true;
					return result;
				}
			}
			foreach (Scenario current2 in ScenarioFiles.AllScenariosLocal)
			{
				if (scen == current2)
				{
					bool result = true;
					return result;
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
			foreach (Scenario current in ScenarioLister.AllScenarios())
			{
				num ^= 791 * current.GetHashCode() * 6121;
			}
			return num;
		}
	}
}
