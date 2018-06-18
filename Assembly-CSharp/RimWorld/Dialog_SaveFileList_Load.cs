using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000812 RID: 2066
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x06002E20 RID: 11808 RVA: 0x00184B39 File Offset: 0x00182F39
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x00184B52 File Offset: 0x00182F52
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
