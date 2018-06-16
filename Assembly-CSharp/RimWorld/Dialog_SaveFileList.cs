using System;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000811 RID: 2065
	public abstract class Dialog_SaveFileList : Dialog_FileList
	{
		// Token: 0x06002E1A RID: 11802 RVA: 0x00184978 File Offset: 0x00182D78
		protected override Color FileNameColor(SaveFileInfo sfi)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sfi.FileInfo.Name);
			if (SaveGameFilesUtility.IsAutoSave(fileNameWithoutExtension))
			{
				GUI.color = Dialog_SaveFileList.AutosaveTextColor;
			}
			return base.FileNameColor(sfi);
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x001849BC File Offset: 0x00182DBC
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

		// Token: 0x06002E1C RID: 11804 RVA: 0x00184A6C File Offset: 0x00182E6C
		public override void PostClose()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x04001885 RID: 6277
		private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);
	}
}
