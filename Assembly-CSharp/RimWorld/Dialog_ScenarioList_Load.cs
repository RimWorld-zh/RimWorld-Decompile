using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000816 RID: 2070
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x06002E28 RID: 11816 RVA: 0x00184D5D File Offset: 0x0018315D
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x00184D80 File Offset: 0x00183180
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

		// Token: 0x04001887 RID: 6279
		private Action<Scenario> scenarioReturner;
	}
}
