using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		private Scenario savingScen;

		public Dialog_ScenarioList_Save(Scenario scen)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = scen.name;
			this.savingScen = scen;
		}

		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

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

		[CompilerGenerated]
		private sealed class <DoFileInteraction>c__AnonStorey0
		{
			internal string absPath;

			internal Dialog_ScenarioList_Save $this;

			public <DoFileInteraction>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				GameDataSaveLoader.SaveScenario(this.$this.savingScen, this.absPath);
			}
		}
	}
}
