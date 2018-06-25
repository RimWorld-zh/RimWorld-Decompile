using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F40 RID: 3904
	public static class GenFile
	{
		// Token: 0x06005E1A RID: 24090 RVA: 0x002FDF58 File Offset: 0x002FC358
		public static string TextFromRawFile(string filePath)
		{
			return File.ReadAllText(filePath);
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x002FDF74 File Offset: 0x002FC374
		public static string TextFromResourceFile(string filePath)
		{
			TextAsset textAsset = Resources.Load("Text/" + filePath) as TextAsset;
			string result;
			if (textAsset == null)
			{
				Log.Message("Found no text asset in resources at " + filePath, false);
				result = null;
			}
			else
			{
				result = GenFile.GetTextWithoutBOM(textAsset);
			}
			return result;
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x002FDFCC File Offset: 0x002FC3CC
		public static string GetTextWithoutBOM(TextAsset textAsset)
		{
			string result = null;
			using (MemoryStream memoryStream = new MemoryStream(textAsset.bytes))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x06005E1D RID: 24093 RVA: 0x002FE044 File Offset: 0x002FC444
		public static IEnumerable<string> LinesFromFile(string filePath)
		{
			string rawText = GenFile.TextFromResourceFile(filePath);
			foreach (string line in GenText.LinesFromString(rawText))
			{
				yield return line;
			}
			yield break;
		}

		// Token: 0x06005E1E RID: 24094 RVA: 0x002FE070 File Offset: 0x002FC470
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
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
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
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
					GenFile.DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs, useLinuxLineEndings);
				}
			}
		}

		// Token: 0x06005E1F RID: 24095 RVA: 0x002FE1C4 File Offset: 0x002FC5C4
		public static string SanitizedFileName(string fileName)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			string text = "";
			for (int i = 0; i < fileName.Length; i++)
			{
				if (!invalidFileNameChars.Contains(fileName[i]))
				{
					text += fileName[i];
				}
			}
			if (text.Length == 0)
			{
				text = "unnamed";
			}
			return text;
		}
	}
}
