using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F3D RID: 3901
	public static class GenFile
	{
		// Token: 0x06005DEA RID: 24042 RVA: 0x002FB7C0 File Offset: 0x002F9BC0
		public static string TextFromRawFile(string filePath)
		{
			return File.ReadAllText(filePath);
		}

		// Token: 0x06005DEB RID: 24043 RVA: 0x002FB7DC File Offset: 0x002F9BDC
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

		// Token: 0x06005DEC RID: 24044 RVA: 0x002FB834 File Offset: 0x002F9C34
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

		// Token: 0x06005DED RID: 24045 RVA: 0x002FB8AC File Offset: 0x002F9CAC
		public static IEnumerable<string> LinesFromFile(string filePath)
		{
			string rawText = GenFile.TextFromResourceFile(filePath);
			foreach (string line in GenText.LinesFromString(rawText))
			{
				yield return line;
			}
			yield break;
		}

		// Token: 0x06005DEE RID: 24046 RVA: 0x002FB8D8 File Offset: 0x002F9CD8
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

		// Token: 0x06005DEF RID: 24047 RVA: 0x002FBA2C File Offset: 0x002F9E2C
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
