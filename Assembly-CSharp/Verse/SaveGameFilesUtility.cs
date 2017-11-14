using System.IO;
using System.Linq;

namespace Verse
{
	public static class SaveGameFilesUtility
	{
		public static bool IsAutoSave(string fileName)
		{
			if (fileName.Length < 8)
			{
				return false;
			}
			return fileName.Substring(0, 8) == "Autosave";
		}

		public static bool SavedGameNamedExists(string fileName)
		{
			foreach (string item in from f in GenFilePaths.AllSavedGameFiles
			select Path.GetFileNameWithoutExtension(f.Name))
			{
				if (item == fileName)
				{
					return true;
				}
			}
			return false;
		}

		public static string UnusedDefaultFileName(string factionLabel)
		{
			string empty = string.Empty;
			int num = 1;
			while (true)
			{
				empty = factionLabel + num.ToString();
				num++;
				if (!SaveGameFilesUtility.SavedGameNamedExists(empty))
					break;
			}
			return empty;
		}
	}
}
