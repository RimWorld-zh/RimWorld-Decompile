using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Verse
{
	public static class LogSimple
	{
		private static List<string> messages = new List<string>();

		private static int tabDepth = 0;

		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		public static void FlushToFileAndOpen()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
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

		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			string text = LogSimple.CompiledLog();
			Log.Message(text, false);
			LogSimple.messages.Clear();
		}

		private static string CompiledLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in LogSimple.messages)
			{
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString().TrimEnd(new char[0]);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static LogSimple()
		{
		}

		[CompilerGenerated]
		private sealed class <FlushToFileAndOpen>c__AnonStorey0
		{
			internal string path;

			public <FlushToFileAndOpen>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Application.OpenURL(this.path);
			}
		}
	}
}
