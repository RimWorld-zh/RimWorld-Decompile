using System;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080F RID: 2063
	public abstract class Dialog_SaveFileList : Dialog_FileList
	{
		// Token: 0x04001883 RID: 6275
		private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);

		// Token: 0x06002E19 RID: 11801 RVA: 0x00184D34 File Offset: 0x00183134
		protected override Color FileNameColor(SaveFileInfo sfi)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sfi.FileInfo.Name);
			if (SaveGameFilesUtility.IsAutoSave(fileNameWithoutExtension))
			{
				GUI.color = Dialog_SaveFileList.AutosaveTextColor;
			}
			return base.FileNameColor(sfi);
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x00184D78 File Offset: 0x00183178
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

		// Token: 0x06002E1B RID: 11803 RVA: 0x00184E28 File Offset: 0x00183228
		public override void PostClose()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}
	}
}
