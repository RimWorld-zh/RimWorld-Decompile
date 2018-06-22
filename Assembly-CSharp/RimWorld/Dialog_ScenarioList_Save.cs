using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000811 RID: 2065
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x06002E20 RID: 11808 RVA: 0x00184F00 File Offset: 0x00183300
		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002E21 RID: 11809 RVA: 0x00184F2C File Offset: 0x0018332C
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x00184F44 File Offset: 0x00183344
		protected override void DoFileInteraction(string fileName)
		{
			string absPath = GenFilePaths.AbsPathForScenario(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveScenario(this.savingScen, absPath);
			}, "SavingLongEvent", false, null);
			Messages.Message("SavedAs".Translate(new object[]
			{
				fileName
			}), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x04001884 RID: 6276
		private Scenario savingScen;
	}
}
