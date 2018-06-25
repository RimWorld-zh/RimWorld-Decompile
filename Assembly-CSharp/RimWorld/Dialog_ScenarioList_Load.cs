using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000814 RID: 2068
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x04001889 RID: 6281
		private Action<Scenario> scenarioReturner;

		// Token: 0x06002E26 RID: 11814 RVA: 0x0018537D File Offset: 0x0018377D
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x001853A0 File Offset: 0x001837A0
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
