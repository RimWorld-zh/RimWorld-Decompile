using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		public Dialog_SaveFileList_Save()
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.bottomAreaHeight = 85f;
			if (Faction.OfPlayer.HasName)
			{
				this.typingName = Faction.OfPlayer.Name;
			}
			else
			{
				this.typingName = SaveGameFilesUtility.UnusedDefaultFileName(Faction.OfPlayer.def.LabelCap);
			}
		}

		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		protected override void DoFileInteraction(string mapName)
		{
			mapName = GenFile.SanitizedFileName(mapName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveGame(mapName);
			}, "SavingLongEvent", false, null);
			Messages.Message("SavedAs".Translate(new object[]
			{
				mapName
			}), MessageTypeDefOf.SilentInput, false);
			PlayerKnowledgeDatabase.Save();
			this.Close(true);
		}

		[CompilerGenerated]
		private sealed class <DoFileInteraction>c__AnonStorey0
		{
			internal string mapName;

			public <DoFileInteraction>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				GameDataSaveLoader.SaveGame(this.mapName);
			}
		}
	}
}
