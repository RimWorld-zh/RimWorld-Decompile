using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000815 RID: 2069
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x06002E25 RID: 11813 RVA: 0x00184C94 File Offset: 0x00183094
		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002E26 RID: 11814 RVA: 0x00184CC0 File Offset: 0x001830C0
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x00184CD8 File Offset: 0x001830D8
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
