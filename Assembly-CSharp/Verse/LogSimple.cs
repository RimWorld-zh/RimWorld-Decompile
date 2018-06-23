using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F17 RID: 3863
	public static class LogSimple
	{
		// Token: 0x04003D8C RID: 15756
		private static List<string> messages = new List<string>();

		// Token: 0x04003D8D RID: 15757
		private static int tabDepth = 0;

		// Token: 0x06005CB6 RID: 23734 RVA: 0x002F0A60 File Offset: 0x002EEE60
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x002F0A9C File Offset: 0x002EEE9C
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x002F0AB1 File Offset: 0x002EEEB1
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x002F0AC0 File Offset: 0x002EEEC0
		public static void FlushToFileAndOpen()
		{
			if (LogSimple.messages.Count != 0)
			{
				string value = LogSimple.CompiledLog();
				string path = GenFilePaths.SaveDataFolderPath + Path.DirectorySeparatorChar + "LogSimple.txt";
				using (StreamWriter streamWriter = new StreamWriter(path, false))
				{
					streamWriter.Write(value);
				}
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					Application.OpenURL(path);
				});
				LogSimple.messages.Clear();
			}
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x002F0B60 File Offset: 0x002EEF60
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count != 0)
			{
				string text = LogSimple.CompiledLog();
				Log.Message(text, false);
				LogSimple.messages.Clear();
			}
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x002F0B9C File Offset: 0x002EEF9C
		private static string CompiledLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in LogSimple.messages)
			{
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString().TrimEnd(new char[0]);
		}
	}
}
