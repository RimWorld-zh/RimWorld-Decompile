using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000812 RID: 2066
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x06002E1E RID: 11806 RVA: 0x00184AA5 File Offset: 0x00182EA5
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x00184ABE File Offset: 0x00182EBE
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
