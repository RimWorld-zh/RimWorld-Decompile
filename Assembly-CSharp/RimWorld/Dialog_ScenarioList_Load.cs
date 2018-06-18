using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000816 RID: 2070
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x06002E2A RID: 11818 RVA: 0x00184DF1 File Offset: 0x001831F1
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x00184E14 File Offset: 0x00183214
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
