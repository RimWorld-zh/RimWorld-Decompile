using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000811 RID: 2065
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		// Token: 0x06002E1E RID: 11806 RVA: 0x001850E8 File Offset: 0x001834E8
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
		// (get) Token: 0x06002E1F RID: 11807 RVA: 0x00185158 File Offset: 0x00183558
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x00185170 File Offset: 0x00183570
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
