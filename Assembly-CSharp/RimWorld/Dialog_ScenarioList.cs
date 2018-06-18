using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000814 RID: 2068
	public abstract class Dialog_ScenarioList : Dialog_FileList
	{
		// Token: 0x06002E26 RID: 11814 RVA: 0x00184C78 File Offset: 0x00183078
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomScenarioFiles)
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
	}
}
