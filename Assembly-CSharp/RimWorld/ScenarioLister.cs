using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000656 RID: 1622
	public static class ScenarioLister
	{
		// Token: 0x060021CA RID: 8650 RVA: 0x0011E070 File Offset: 0x0011C470
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

		// Token: 0x060021CB RID: 8651 RVA: 0x0011E094 File Offset: 0x0011C494
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

		// Token: 0x060021CC RID: 8652 RVA: 0x0011E0C0 File Offset: 0x0011C4C0
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

		// Token: 0x060021CD RID: 8653 RVA: 0x0011E18C File Offset: 0x0011C58C
		public static void MarkDirty()
		{
			ScenarioLister.dirty = true;
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x0011E195 File Offset: 0x0011C595
		private static void RecacheIfDirty()
		{
			if (ScenarioLister.dirty)
			{
				ScenarioLister.RecacheData();
			}
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x0011E1A8 File Offset: 0x0011C5A8
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

		// Token: 0x060021D0 RID: 8656 RVA: 0x0011E1F8 File Offset: 0x0011C5F8
		public static int ScenarioListHash()
		{
			int num = 9826121;
			foreach (Scenario scenario in ScenarioLister.AllScenarios())
			{
				num ^= 791 * scenario.GetHashCode() * 6121;
			}
			return num;
		}

		// Token: 0x0400132D RID: 4909
		private static bool dirty = true;
	}
}
