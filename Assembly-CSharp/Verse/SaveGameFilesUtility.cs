using System;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D92 RID: 3474
	public static class SaveGameFilesUtility
	{
		// Token: 0x06004DA0 RID: 19872 RVA: 0x002882B0 File Offset: 0x002866B0
		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x002882EC File Offset: 0x002866EC
		public static bool SavedGameNamedExists(string fileName)
		{
			foreach (string a in from f in GenFilePaths.AllSavedGameFiles
			select Path.GetFileNameWithoutExtension(f.Name))
			{
				if (a == fileName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x00288380 File Offset: 0x00286780
		public static string UnusedDefaultFileName(string factionLabel)
		{
			int num = 1;
			string text;
			do
			{
				text = factionLabel + num.ToString();
				num++;
			}
			while (SaveGameFilesUtility.SavedGameNamedExists(text));
			return text;
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x002883C4 File Offset: 0x002867C4
		public static FileInfo GetAutostartSaveFile()
		{
			FileInfo result;
			if (!Prefs.DevMode)
			{
				result = null;
			}
			else
			{
				result = GenFilePaths.AllSavedGameFiles.FirstOrDefault((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name).ToLower() == "autostart".ToLower());
			}
			return result;
		}
	}
}
