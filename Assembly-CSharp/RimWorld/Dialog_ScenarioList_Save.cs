using System;
using Verse;

namespace RimWorld
{
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		private Scenario savingScen;

		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		public Dialog_ScenarioList_Save(Scenario scen)
		{
			base.interactButLabel = "OverwriteButton".Translate();
			base.typingName = scen.name;
			this.savingScen = scen;
		}

		protected override void DoFileInteraction(string fileName)
		{
			string absPath = GenFilePaths.AbsPathForScenario(fileName);
			LongEventHandler.QueueLongEvent((Action)delegate
			{
				GameDataSaveLoader.SaveScenario(this.savingScen, absPath);
			}, "SavingLongEvent", false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageSound.Silent);
			this.Close(true);
		}
	}
}
