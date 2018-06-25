using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000813 RID: 2067
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x04001884 RID: 6276
		private Scenario savingScen;

		// Token: 0x06002E24 RID: 11812 RVA: 0x00185050 File Offset: 0x00183450
		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002E25 RID: 11813 RVA: 0x0018507C File Offset: 0x0018347C
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x00185094 File Offset: 0x00183494
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
