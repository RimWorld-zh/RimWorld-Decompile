using System;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080D RID: 2061
	public abstract class Dialog_SaveFileList : Dialog_FileList
	{
		// Token: 0x04001883 RID: 6275
		private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);

		// Token: 0x06002E15 RID: 11797 RVA: 0x00184BE4 File Offset: 0x00182FE4
		protected override Color FileNameColor(SaveFileInfo sfi)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sfi.FileInfo.Name);
			if (SaveGameFilesUtility.IsAutoSave(fileNameWithoutExtension))
			{
				GUI.color = Dialog_SaveFileList.AutosaveTextColor;
			}
			return base.FileNameColor(sfi);
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x00184C28 File Offset: 0x00183028
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllSavedGameFiles)
			{
				try
				{
					this.files.Add(new SaveFileInfo(fileInfo));
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString(), false);
				}
			}
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x00184CD8 File Offset: 0x001830D8
		public override void PostClose()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}
	}
}
