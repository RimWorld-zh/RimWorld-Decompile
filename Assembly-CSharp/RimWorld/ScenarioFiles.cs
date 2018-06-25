using System;
using System.Collections.Generic;
using System.IO;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02000653 RID: 1619
	public static class ScenarioFiles
	{
		// Token: 0x0400132C RID: 4908
		private static List<Scenario> scenariosLocal = new List<Scenario>();

		// Token: 0x0400132D RID: 4909
		private static List<Scenario> scenariosWorkshop = new List<Scenario>();

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060021C3 RID: 8643 RVA: 0x0011E460 File Offset: 0x0011C860
		public static IEnumerable<Scenario> AllScenariosLocal
		{
			get
			{
				return ScenarioFiles.scenariosLocal;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x0011E47C File Offset: 0x0011C87C
		public static IEnumerable<Scenario> AllScenariosWorkshop
		{
			get
			{
				return ScenarioFiles.scenariosWorkshop;
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x0011E498 File Offset: 0x0011C898
		public static void RecacheData()
		{
			ScenarioFiles.scenariosLocal.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomScenarioFiles)
			{
				Scenario item;
				if (GameDataSaveLoader.TryLoadScenario(fileInfo.FullName, ScenarioCategory.CustomLocal, out item))
				{
					ScenarioFiles.scenariosLocal.Add(item);
				}
			}
			ScenarioFiles.scenariosWorkshop.Clear();
			foreach (WorkshopItem workshopItem in WorkshopItems.AllSubscribedItems)
			{
				WorkshopItem_Scenario workshopItem_Scenario = workshopItem as WorkshopItem_Scenario;
				if (workshopItem_Scenario != null)
				{
					ScenarioFiles.scenariosWorkshop.Add(workshopItem_Scenario.GetScenario());
				}
			}
		}
	}
}
