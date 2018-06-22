using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CD RID: 2509
	public static class LastPlayedVersion
	{
		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06003846 RID: 14406 RVA: 0x001E0084 File Offset: 0x001DE484
		public static Version Version
		{
			get
			{
				LastPlayedVersion.InitializeIfNeeded();
				return LastPlayedVersion.lastPlayedVersionInt;
			}
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x001E00A4 File Offset: 0x001DE4A4
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

		// Token: 0x04002400 RID: 9216
		private static bool initialized = false;

		// Token: 0x04002401 RID: 9217
		private static Version lastPlayedVersionInt = null;
	}
}
