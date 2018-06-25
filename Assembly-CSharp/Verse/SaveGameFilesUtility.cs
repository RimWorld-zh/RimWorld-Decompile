using System;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D91 RID: 3473
	public static class SaveGameFilesUtility
	{
		// Token: 0x06004DB9 RID: 19897 RVA: 0x0028998C File Offset: 0x00287D8C
		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x002899C8 File Offset: 0x00287DC8
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

		// Token: 0x06004DBB RID: 19899 RVA: 0x00289A5C File Offset: 0x00287E5C
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

		// Token: 0x06004DBC RID: 19900 RVA: 0x00289AA0 File Offset: 0x00287EA0
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
