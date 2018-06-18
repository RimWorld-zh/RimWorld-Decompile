using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D1 RID: 2513
	public static class LastPlayedVersion
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x0600384C RID: 14412 RVA: 0x001DFEAC File Offset: 0x001DE2AC
		public static Version Version
		{
			get
			{
				LastPlayedVersion.InitializeIfNeeded();
				return LastPlayedVersion.lastPlayedVersionInt;
			}
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x001DFECC File Offset: 0x001DE2CC
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

		// Token: 0x04002405 RID: 9221
		private static bool initialized = false;

		// Token: 0x04002406 RID: 9222
		private static Version lastPlayedVersionInt = null;
	}
}
