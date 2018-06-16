using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F18 RID: 3864
	public static class LogSimple
	{
		// Token: 0x06005C90 RID: 23696 RVA: 0x002EE958 File Offset: 0x002ECD58
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x002EE994 File Offset: 0x002ECD94
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x06005C92 RID: 23698 RVA: 0x002EE9A9 File Offset: 0x002ECDA9
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x06005C93 RID: 23699 RVA: 0x002EE9B8 File Offset: 0x002ECDB8
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

		// Token: 0x06005C94 RID: 23700 RVA: 0x002EEA58 File Offset: 0x002ECE58
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count != 0)
			{
				string text = LogSimple.CompiledLog();
				Log.Message(text, false);
				LogSimple.messages.Clear();
			}
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x002EEA94 File Offset: 0x002ECE94
		private static string CompiledLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in LogSimple.messages)
			{
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString().TrimEnd(new char[0]);
		}

		// Token: 0x04003D7B RID: 15739
		private static List<string> messages = new List<string>();

		// Token: 0x04003D7C RID: 15740
		private static int tabDepth = 0;
	}
}
