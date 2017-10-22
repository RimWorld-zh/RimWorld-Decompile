using System.Collections.Generic;
using System.IO;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	public static class ScenarioFiles
	{
		private static List<Scenario> scenariosLocal = new List<Scenario>();

		private static List<Scenario> scenariosWorkshop = new List<Scenario>();

		public static IEnumerable<Scenario> AllScenariosLocal
		{
			get
			{
				return ScenarioFiles.scenariosLocal;
			}
		}

		public static IEnumerable<Scenario> AllScenariosWorkshop
		{
			get
			{
				return ScenarioFiles.scenariosWorkshop;
			}
		}

		public static void RecacheData()
		{
			ScenarioFiles.scenariosLocal.Clear();
			foreach (FileInfo allCustomScenarioFile in GenFilePaths.AllCustomScenarioFiles)
			{
				Scenario item = default(Scenario);
				if (GameDataSaveLoader.TryLoadScenario(allCustomScenarioFile.FullName, ScenarioCategory.CustomLocal, out item))
				{
					ScenarioFiles.scenariosLocal.Add(item);
				}
			}
			ScenarioFiles.scenariosWorkshop.Clear();
			foreach (WorkshopItem allSubscribedItem in WorkshopItems.AllSubscribedItems)
			{
				WorkshopItem_Scenario workshopItem_Scenario = allSubscribedItem as WorkshopItem_Scenario;
				if (workshopItem_Scenario != null)
				{
					ScenarioFiles.scenariosWorkshop.Add(workshopItem_Scenario.GetScenario());
				}
			}
		}
	}
}
