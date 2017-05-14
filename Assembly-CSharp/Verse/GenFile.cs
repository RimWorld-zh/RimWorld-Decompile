using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			if (textAsset == null)
			{
				Log.Message("Found no text asset in resources at " + filePath);
				return null;
			}
			return GenFile.GetTextWithoutBOM(textAsset);
		}

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

		[DebuggerHidden]
		public static IEnumerable<string> LinesFromFile(string filePath)
		{
			GenFile.<LinesFromFile>c__Iterator247 <LinesFromFile>c__Iterator = new GenFile.<LinesFromFile>c__Iterator247();
			<LinesFromFile>c__Iterator.filePath = filePath;
			<LinesFromFile>c__Iterator.<$>filePath = filePath;
			GenFile.<LinesFromFile>c__Iterator247 expr_15 = <LinesFromFile>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
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
			FileInfo[] files = directoryInfo.GetFiles();
			FileInfo[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				FileInfo fileInfo = array[i];
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
