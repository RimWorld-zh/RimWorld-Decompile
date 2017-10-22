using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	public static class GenFile
	{
		public static string TextFromRawFile(string filePath)
		{
			return File.ReadAllText(filePath);
		}

		public static string TextFromResourceFile(string filePath)
		{
			TextAsset textAsset = Resources.Load("Text/" + filePath) as TextAsset;
			if ((Object)textAsset == (Object)null)
			{
				Log.Message("Found no text asset in resources at " + filePath);
				return (string)null;
			}
			return GenFile.GetTextWithoutBOM(textAsset);
		}

		public static string GetTextWithoutBOM(TextAsset textAsset)
		{
			string text = (string)null;
			using (MemoryStream stream = new MemoryStream(textAsset.bytes))
			{
				using (StreamReader streamReader = new StreamReader(stream, true))
				{
					return streamReader.ReadToEnd();
				}
			}
		}

		public static IEnumerable<string> LinesFromFile(string filePath)
		{
			string rawText = GenFile.TextFromResourceFile(filePath);
			foreach (string item in GenText.LinesFromString(rawText))
			{
				yield return item;
			}
		}

		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			FileInfo[] files;
			FileInfo[] array = files = directoryInfo.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				FileInfo fileInfo = files[i];
				string destFileName = Path.Combine(destDirName, fileInfo.Name);
				fileInfo.CopyTo(destFileName, false);
			}
			if (copySubDirs)
			{
				DirectoryInfo[] array2 = directories;
				for (int j = 0; j < array2.Length; j++)
				{
					DirectoryInfo directoryInfo2 = array2[j];
					string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
					GenFile.DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs);
				}
			}
		}
	}
}
