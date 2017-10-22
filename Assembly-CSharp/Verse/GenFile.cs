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
			string result;
			if ((Object)textAsset == (Object)null)
			{
				Log.Message("Found no text asset in resources at " + filePath);
				result = (string)null;
			}
			else
			{
				result = GenFile.GetTextWithoutBOM(textAsset);
			}
			return result;
		}

		public static string GetTextWithoutBOM(TextAsset textAsset)
		{
			string result = (string)null;
			using (MemoryStream stream = new MemoryStream(textAsset.bytes))
			{
				using (StreamReader streamReader = new StreamReader(stream, true))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		public static IEnumerable<string> LinesFromFile(string filePath)
		{
			string rawText = GenFile.TextFromResourceFile(filePath);
			using (IEnumerator<string> enumerator = GenText.LinesFromString(rawText).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string line = enumerator.Current;
					yield return line;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00ce:
			/*Error near IL_00cf: Unexpected return in MoveNext()*/;
		}

		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool useLinuxLineEndings = false)
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
				string text = Path.Combine(destDirName, fileInfo.Name);
				if (useLinuxLineEndings && (fileInfo.Extension == ".sh" || fileInfo.Extension == ".txt"))
				{
					if (!File.Exists(text))
					{
						File.WriteAllText(text, File.ReadAllText(fileInfo.FullName).Replace("\r\n", "\n").Replace("\r", "\n"));
					}
				}
				else
				{
					fileInfo.CopyTo(text, false);
				}
			}
			if (copySubDirs)
			{
				DirectoryInfo[] array2 = directories;
				for (int j = 0; j < array2.Length; j++)
				{
					DirectoryInfo directoryInfo2 = array2[j];
					string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
					GenFile.DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs, useLinuxLineEndings);
				}
			}
		}
	}
}
