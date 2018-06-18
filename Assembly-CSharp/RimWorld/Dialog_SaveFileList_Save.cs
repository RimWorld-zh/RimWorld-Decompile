using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000813 RID: 2067
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		// Token: 0x06002E22 RID: 11810 RVA: 0x00184B5C File Offset: 0x00182F5C
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
		// (get) Token: 0x06002E23 RID: 11811 RVA: 0x00184BCC File Offset: 0x00182FCC
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x00184BE4 File Offset: 0x00182FE4
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
