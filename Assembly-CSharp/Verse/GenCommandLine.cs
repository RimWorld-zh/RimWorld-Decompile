using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Verse
{
	// Token: 0x02000F36 RID: 3894
	public static class GenCommandLine
	{
		// Token: 0x06005DBD RID: 23997 RVA: 0x002F9E80 File Offset: 0x002F8280
		public static bool CommandLineArgPassed(string key)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (string.Compare(commandLineArgs[i], key, true) == 0 || string.Compare(commandLineArgs[i], "-" + key, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005DBE RID: 23998 RVA: 0x002F9EE4 File Offset: 0x002F82E4
		public static bool TryGetCommandLineArg(string key, out string value)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int i = 0;
			while (i < commandLineArgs.Length)
			{
				if (commandLineArgs[i].Contains('='))
				{
					string[] array = commandLineArgs[i].Split(new char[]
					{
						'='
					});
					if (array.Length == 2)
					{
						if (string.Compare(array[0], key, true) == 0 || string.Compare(array[0], "-" + key, true) == 0)
						{
							value = array[1];
							return true;
						}
					}
				}
				IL_78:
				i++;
				continue;
				goto IL_78;
			}
			value = null;
			return false;
		}

		// Token: 0x06005DBF RID: 23999 RVA: 0x002F9F84 File Offset: 0x002F8384
		public static void Restart()
		{
			try
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				string text = commandLineArgs[0];
				string text2 = "";
				for (int i = 1; i < commandLineArgs.Length; i++)
				{
					if (!text2.NullOrEmpty())
					{
						text2 += " ";
					}
					text2 = text2 + "\"" + commandLineArgs[i].Replace("\"", "\\\"") + "\"";
				}
				new Process
				{
					StartInfo = new ProcessStartInfo(commandLineArgs[0], text2)
				}.Start();
				Root.Shutdown();
				LongEventHandler.QueueLongEvent(delegate()
				{
					Thread.Sleep(10000);
				}, "Restarting", true, null);
			}
			catch (Exception arg)
			{
				Log.Error("Error restarting: " + arg, false);
				Find.WindowStack.Add(new Dialog_MessageBox("FailedToRestart".Translate(), null, null, null, null, null, false, null, null));
			}
		}
	}
}
