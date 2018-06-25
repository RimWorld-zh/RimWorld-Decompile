using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000654 RID: 1620
	public static class ScenarioLister
	{
		// Token: 0x0400132E RID: 4910
		private static bool dirty = true;

		// Token: 0x060021C7 RID: 8647 RVA: 0x0011E5A0 File Offset: 0x0011C9A0
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

		// Token: 0x060021C8 RID: 8648 RVA: 0x0011E5C4 File Offset: 0x0011C9C4
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

		// Token: 0x060021C9 RID: 8649 RVA: 0x0011E5F0 File Offset: 0x0011C9F0
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

		// Token: 0x060021CA RID: 8650 RVA: 0x0011E6BC File Offset: 0x0011CABC
		public static void MarkDirty()
		{
			ScenarioLister.dirty = true;
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x0011E6C5 File Offset: 0x0011CAC5
		private static void RecacheIfDirty()
		{
			if (ScenarioLister.dirty)
			{
				ScenarioLister.RecacheData();
			}
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x0011E6D8 File Offset: 0x0011CAD8
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

		// Token: 0x060021CD RID: 8653 RVA: 0x0011E728 File Offset: 0x0011CB28
		public static int ScenarioListHash()
		{
			int num = 9826121;
			foreach (Scenario scenario in ScenarioLister.AllScenarios())
			{
				num ^= 791 * scenario.GetHashCode() * 6121;
			}
			return num;
		}
	}
}
