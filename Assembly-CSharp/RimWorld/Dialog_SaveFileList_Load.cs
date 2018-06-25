using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000810 RID: 2064
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x06002E1D RID: 11805 RVA: 0x00184E61 File Offset: 0x00183261
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x00184E7A File Offset: 0x0018327A
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
