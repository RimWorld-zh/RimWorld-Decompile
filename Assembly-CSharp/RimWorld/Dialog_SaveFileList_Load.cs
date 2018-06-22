using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080E RID: 2062
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x06002E19 RID: 11801 RVA: 0x00184D11 File Offset: 0x00183111
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x00184D2A File Offset: 0x0018312A
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
