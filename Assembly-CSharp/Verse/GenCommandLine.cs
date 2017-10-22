using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Verse
{
	public static class GenCommandLine
	{
		public static bool CommandLineArgPassed(string key)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int num = 0;
			bool result;
			while (true)
			{
				if (num < commandLineArgs.Length)
				{
					if (((string.Compare(commandLineArgs[num], key, true) != 0) ? string.Compare(commandLineArgs[num], "-" + key, true) : 0) != 0)
					{
						num++;
						continue;
					}
					result = true;
				}
				else
				{
					result = false;
				}
				break;
			}
			return result;
		}

		public static bool TryGetCommandLineArg(string key, out string value)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int num = 0;
			bool result;
			while (true)
			{
				if (num < commandLineArgs.Length)
				{
					if (commandLineArgs[num].Contains('='))
					{
						string[] array = commandLineArgs[num].Split('=');
						if (array.Length == 2 && (string.Compare(array[0], key, true) == 0 || string.Compare(array[0], "-" + key, true) == 0))
						{
							value = array[1];
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				value = (string)null;
				result = false;
				break;
			}
			return result;
		}

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
				Process process = new Process();
				process.StartInfo = new ProcessStartInfo(commandLineArgs[0], text2);
				process.Start();
				Root.Shutdown();
				LongEventHandler.QueueLongEvent((Action)delegate
				{
					Thread.Sleep(10000);
				}, "Restarting", true, null);
			}
			catch (Exception arg)
			{
				Log.Error("Error restarting: " + arg);
				Find.WindowStack.Add(new Dialog_MessageBox("FailedToRestart".Translate(), (string)null, null, (string)null, null, (string)null, false));
			}
		}
	}
}
