using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000812 RID: 2066
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x04001885 RID: 6277
		private Action<Scenario> scenarioReturner;

		// Token: 0x06002E23 RID: 11811 RVA: 0x00184FC9 File Offset: 0x001833C9
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x00184FEC File Offset: 0x001833EC
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
