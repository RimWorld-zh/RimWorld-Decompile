using System;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D8F RID: 3471
	public static class SaveGameFilesUtility
	{
		// Token: 0x06004DB5 RID: 19893 RVA: 0x00289860 File Offset: 0x00287C60
		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

		// Token: 0x06004DB6 RID: 19894 RVA: 0x0028989C File Offset: 0x00287C9C
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

		// Token: 0x06004DB7 RID: 19895 RVA: 0x00289930 File Offset: 0x00287D30
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

		// Token: 0x06004DB8 RID: 19896 RVA: 0x00289974 File Offset: 0x00287D74
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
