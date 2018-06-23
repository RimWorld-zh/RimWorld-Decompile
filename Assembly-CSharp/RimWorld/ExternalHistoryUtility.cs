using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F7 RID: 759
	public static class ExternalHistoryUtility
	{
		// Token: 0x04000846 RID: 2118
		private static List<FileInfo> cachedFiles;

		// Token: 0x04000847 RID: 2119
		private static int gameplayIDLength = 20;

		// Token: 0x04000848 RID: 2120
		private static string gameplayIDAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0006F83C File Offset: 0x0006DC3C
		static ExternalHistoryUtility()
		{
			try
			{
				ExternalHistoryUtility.cachedFiles = GenFilePaths.AllExternalHistoryFiles.ToList<FileInfo>();
			}
			catch (Exception ex)
			{
				Log.Error("Could not get external history files: " + ex.Message, false);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0006F8A0 File Offset: 0x0006DCA0
		public static IEnumerable<FileInfo> Files
		{
			get
			{
				for (int i = 0; i < ExternalHistoryUtility.cachedFiles.Count; i++)
				{
					yield return ExternalHistoryUtility.cachedFiles[i];
				}
				yield break;
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0006F8C4 File Offset: 0x0006DCC4
		public static ExternalHistory Load(string path)
		{
			ExternalHistory result = null;
			try
			{
				result = new ExternalHistory();
				Scribe.loader.InitLoading(path);
				try
				{
					Scribe_Deep.Look<ExternalHistory>(ref result, "externalHistory", new object[0]);
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Could not load external history (" + path + "): " + ex.Message, false);
				return null;
			}
			return result;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0006F964 File Offset: 0x0006DD64
		public static string GetRandomGameplayID()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < ExternalHistoryUtility.gameplayIDLength; i++)
			{
				int index = Rand.Range(0, ExternalHistoryUtility.gameplayIDAvailableChars.Length);
				stringBuilder.Append(ExternalHistoryUtility.gameplayIDAvailableChars[index]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0006F9C0 File Offset: 0x0006DDC0
		public static bool IsValidGameplayID(string ID)
		{
			bool result;
			if (ID.NullOrEmpty() || ID.Length != ExternalHistoryUtility.gameplayIDLength)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < ID.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < ExternalHistoryUtility.gameplayIDAvailableChars.Length; j++)
					{
						if (ID[i] == ExternalHistoryUtility.gameplayIDAvailableChars[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0006FA5C File Offset: 0x0006DE5C
		public static string GetCurrentUploadDate()
		{
			return DateTime.UtcNow.ToString("yyMMdd");
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0006FA84 File Offset: 0x0006DE84
		public static int GetCurrentUploadTime()
		{
			return (int)(DateTime.UtcNow.TimeOfDay.TotalSeconds / 2.0);
		}
	}
}
