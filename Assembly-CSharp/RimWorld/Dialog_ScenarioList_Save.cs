using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000815 RID: 2069
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x06002E27 RID: 11815 RVA: 0x00184D28 File Offset: 0x00183128
		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002E28 RID: 11816 RVA: 0x00184D54 File Offset: 0x00183154
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x00184D6C File Offset: 0x0018316C
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

		// Token: 0x04001886 RID: 6278
		private Scenario savingScen;
	}
}
