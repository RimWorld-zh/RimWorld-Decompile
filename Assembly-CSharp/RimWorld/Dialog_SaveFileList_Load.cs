using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000810 RID: 2064
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x06002E1C RID: 11804 RVA: 0x001850C5 File Offset: 0x001834C5
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x001850DE File Offset: 0x001834DE
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
