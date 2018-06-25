using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F9 RID: 761
	public static class ExternalHistoryUtility
	{
		// Token: 0x04000846 RID: 2118
		private static List<FileInfo> cachedFiles;

		// Token: 0x04000847 RID: 2119
		private static int gameplayIDLength = 20;

		// Token: 0x04000848 RID: 2120
		private static string gameplayIDAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

		// Token: 0x06000CAB RID: 3243 RVA: 0x0006F98C File Offset: 0x0006DD8C
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
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0006F9F0 File Offset: 0x0006DDF0
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

		// Token: 0x06000CAD RID: 3245 RVA: 0x0006FA14 File Offset: 0x0006DE14
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

		// Token: 0x06000CAE RID: 3246 RVA: 0x0006FAB4 File Offset: 0x0006DEB4
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

		// Token: 0x06000CAF RID: 3247 RVA: 0x0006FB10 File Offset: 0x0006DF10
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

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0006FBAC File Offset: 0x0006DFAC
		public static string GetCurrentUploadDate()
		{
			return DateTime.UtcNow.ToString("yyMMdd");
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0006FBD4 File Offset: 0x0006DFD4
		public static int GetCurrentUploadTime()
		{
			return (int)(DateTime.UtcNow.TimeOfDay.TotalSeconds / 2.0);
		}
	}
}
