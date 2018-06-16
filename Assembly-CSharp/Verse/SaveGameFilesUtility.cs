using System;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D93 RID: 3475
	public static class SaveGameFilesUtility
	{
		// Token: 0x06004DA2 RID: 19874 RVA: 0x002882D0 File Offset: 0x002866D0
		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x0028830C File Offset: 0x0028670C
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

		// Token: 0x06004DA4 RID: 19876 RVA: 0x002883A0 File Offset: 0x002867A0
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

		// Token: 0x06004DA5 RID: 19877 RVA: 0x002883E4 File Offset: 0x002867E4
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
