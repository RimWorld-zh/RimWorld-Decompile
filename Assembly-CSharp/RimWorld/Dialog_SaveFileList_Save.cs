using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080F RID: 2063
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		// Token: 0x06002E1B RID: 11803 RVA: 0x00184D34 File Offset: 0x00183134
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

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002E1C RID: 11804 RVA: 0x00184DA4 File Offset: 0x001831A4
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x00184DBC File Offset: 0x001831BC
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
