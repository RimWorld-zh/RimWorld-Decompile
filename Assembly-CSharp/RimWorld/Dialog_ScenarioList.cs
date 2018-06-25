using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02000812 RID: 2066
	public abstract class Dialog_ScenarioList : Dialog_FileList
	{
		// Token: 0x06002E22 RID: 11810 RVA: 0x00185204 File Offset: 0x00183604
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
