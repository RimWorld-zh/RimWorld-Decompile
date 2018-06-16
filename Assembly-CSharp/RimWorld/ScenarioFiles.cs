using System;
using System.Collections.Generic;
using System.IO;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02000655 RID: 1621
	public static class ScenarioFiles
	{
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060021C6 RID: 8646 RVA: 0x0011DF30 File Offset: 0x0011C330
		public static IEnumerable<Scenario> AllScenariosLocal
		{
			get
			{
				return ScenarioFiles.scenariosLocal;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x0011DF4C File Offset: 0x0011C34C
		public static IEnumerable<Scenario> AllScenariosWorkshop
		{
			get
			{
				return ScenarioFiles.scenariosWorkshop;
			}
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x0011DF68 File Offset: 0x0011C368
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

		// Token: 0x0400132B RID: 4907
		private static List<Scenario> scenariosLocal = new List<Scenario>();

		// Token: 0x0400132C RID: 4908
		private static List<Scenario> scenariosWorkshop = new List<Scenario>();
	}
}
