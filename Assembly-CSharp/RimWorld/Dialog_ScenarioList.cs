using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000810 RID: 2064
	public abstract class Dialog_ScenarioList : Dialog_FileList
	{
		// Token: 0x06002E1F RID: 11807 RVA: 0x00184E50 File Offset: 0x00183250
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
