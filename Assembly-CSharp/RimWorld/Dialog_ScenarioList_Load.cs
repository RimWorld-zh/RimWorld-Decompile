using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000814 RID: 2068
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x04001885 RID: 6277
		private Action<Scenario> scenarioReturner;

		// Token: 0x06002E27 RID: 11815 RVA: 0x00185119 File Offset: 0x00183519
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x0018513C File Offset: 0x0018353C
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsPathForScenario(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, delegate
			{
				Scenario obj = null;
				if (GameDataSaveLoader.TryLoadScenario(filePath, ScenarioCategory.CustomLocal, out obj))
				{
					this.scenarioReturner(obj);
				}
				this.Close(true);
			});
		}
	}
}
