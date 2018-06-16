using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000814 RID: 2068
	public abstract class Dialog_ScenarioList : Dialog_FileList
	{
		// Token: 0x06002E24 RID: 11812 RVA: 0x00184BE4 File Offset: 0x00182FE4
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
