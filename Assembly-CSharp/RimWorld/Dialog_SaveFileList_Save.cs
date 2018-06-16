using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000813 RID: 2067
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		// Token: 0x06002E20 RID: 11808 RVA: 0x00184AC8 File Offset: 0x00182EC8
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

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002E21 RID: 11809 RVA: 0x00184B38 File Offset: 0x00182F38
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x00184B50 File Offset: 0x00182F50
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
	}
}
