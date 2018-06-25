using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F1B RID: 3867
	public static class LogSimple
	{
		// Token: 0x04003D8F RID: 15759
		private static List<string> messages = new List<string>();

		// Token: 0x04003D90 RID: 15760
		private static int tabDepth = 0;

		// Token: 0x06005CC0 RID: 23744 RVA: 0x002F10E0 File Offset: 0x002EF4E0
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x002F111C File Offset: 0x002EF51C
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x002F1131 File Offset: 0x002EF531
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x002F1140 File Offset: 0x002EF540
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

		// Token: 0x06005CC4 RID: 23748 RVA: 0x002F11E0 File Offset: 0x002EF5E0
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count != 0)
			{
				string text = LogSimple.CompiledLog();
				Log.Message(text, false);
				LogSimple.messages.Clear();
			}
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x002F121C File Offset: 0x002EF61C
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
