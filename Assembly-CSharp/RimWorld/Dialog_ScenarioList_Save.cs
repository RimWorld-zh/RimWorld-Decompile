using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000813 RID: 2067
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x04001888 RID: 6280
		private Scenario savingScen;

		// Token: 0x06002E23 RID: 11811 RVA: 0x001852B4 File Offset: 0x001836B4
		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x001852E0 File Offset: 0x001836E0
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x001852F8 File Offset: 0x001836F8
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
	}
}
