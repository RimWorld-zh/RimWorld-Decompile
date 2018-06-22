using System;
using System.Collections.Generic;
using System.IO;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02000651 RID: 1617
	public static class ScenarioFiles
	{
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x0011E0A8 File Offset: 0x0011C4A8
		public static IEnumerable<Scenario> AllScenariosLocal
		{
			get
			{
				return ScenarioFiles.scenariosLocal;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060021C1 RID: 8641 RVA: 0x0011E0C4 File Offset: 0x0011C4C4
		public static IEnumerable<Scenario> AllScenariosWorkshop
		{
			get
			{
				return ScenarioFiles.scenariosWorkshop;
			}
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x0011E0E0 File Offset: 0x0011C4E0
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

		// Token: 0x04001328 RID: 4904
		private static List<Scenario> scenariosLocal = new List<Scenario>();

		// Token: 0x04001329 RID: 4905
		private static List<Scenario> scenariosWorkshop = new List<Scenario>();
	}
}
