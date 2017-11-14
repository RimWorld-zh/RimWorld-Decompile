using Verse;

namespace RimWorld
{
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		public Dialog_SaveFileList_Save()
		{
			base.interactButLabel = "OverwriteButton".Translate();
			base.bottomAreaHeight = 85f;
			if (Faction.OfPlayer.HasName)
			{
				base.typingName = Faction.OfPlayer.Name;
			}
			else
			{
				base.typingName = SaveGameFilesUtility.UnusedDefaultFileName(Faction.OfPlayer.def.LabelCap);
			}
		}

		protected override void DoFileInteraction(string mapName)
		{
			LongEventHandler.QueueLongEvent(delegate
			{
				GameDataSaveLoader.SaveGame(mapName);
			}, "SavingLongEvent", false, null);
			Messages.Message("SavedAs".Translate(mapName), MessageTypeDefOf.SilentInput);
			PlayerKnowledgeDatabase.Save();
			this.Close(true);
		}
	}
}
