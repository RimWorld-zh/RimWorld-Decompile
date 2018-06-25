using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	public static class SaveGameFilesUtility
	{
		[CompilerGenerated]
		private static Func<FileInfo, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<FileInfo, bool> <>f__am$cache1;

		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

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

		[CompilerGenerated]
		private static string <SavedGameNamedExists>m__0(FileInfo f)
		{
			return Path.GetFileNameWithoutExtension(f.Name);
		}

		[CompilerGenerated]
		private static bool <GetAutostartSaveFile>m__1(FileInfo x)
		{
			return Path.GetFileNameWithoutExtension(x.Name).ToLower() == "autostart".ToLower();
		}
	}
}
