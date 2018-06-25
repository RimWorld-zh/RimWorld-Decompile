using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CF RID: 2511
	public static class LastPlayedVersion
	{
		// Token: 0x04002408 RID: 9224
		private static bool initialized = false;

		// Token: 0x04002409 RID: 9225
		private static Version lastPlayedVersionInt = null;

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x0600384A RID: 14410 RVA: 0x001E049C File Offset: 0x001DE89C
		public static Version Version
		{
			get
			{
				LastPlayedVersion.InitializeIfNeeded();
				return LastPlayedVersion.lastPlayedVersionInt;
			}
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x001E04BC File Offset: 0x001DE8BC
		public static void InitializeIfNeeded()
		{
			if (!LastPlayedVersion.initialized)
			{
				try
				{
					string text = null;
					if (File.Exists(GenFilePaths.LastPlayedVersionFilePath))
					{
						try
						{
							text = File.ReadAllText(GenFilePaths.LastPlayedVersionFilePath);
						}
						catch (Exception ex)
						{
							Log.Error("Exception getting last played version data. Path: " + GenFilePaths.LastPlayedVersionFilePath + ". Exception: " + ex.ToString(), false);
						}
					}
					if (text != null)
					{
						try
						{
							LastPlayedVersion.lastPlayedVersionInt = VersionControl.VersionFromString(text);
						}
						catch (Exception ex2)
						{
							Log.Error("Exception parsing last version from string '" + text + "': " + ex2.ToString(), false);
						}
					}
					if (LastPlayedVersion.lastPlayedVersionInt != VersionControl.CurrentVersion)
					{
						File.WriteAllText(GenFilePaths.LastPlayedVersionFilePath, VersionControl.CurrentVersionString);
					}
				}
				finally
				{
					LastPlayedVersion.initialized = true;
				}
			}
		}
	}
}
