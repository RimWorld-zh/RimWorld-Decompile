using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F1C RID: 3868
	public static class LogSimple
	{
		// Token: 0x04003D97 RID: 15767
		private static List<string> messages = new List<string>();

		// Token: 0x04003D98 RID: 15768
		private static int tabDepth = 0;

		// Token: 0x06005CC0 RID: 23744 RVA: 0x002F1300 File Offset: 0x002EF700
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x002F133C File Offset: 0x002EF73C
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x002F1351 File Offset: 0x002EF751
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x002F1360 File Offset: 0x002EF760
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

		// Token: 0x06005CC4 RID: 23748 RVA: 0x002F1400 File Offset: 0x002EF800
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count != 0)
			{
				string text = LogSimple.CompiledLog();
				Log.Message(text, false);
				LogSimple.messages.Clear();
			}
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x002F143C File Offset: 0x002EF83C
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
