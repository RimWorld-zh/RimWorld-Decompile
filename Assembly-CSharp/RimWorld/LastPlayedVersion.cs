using System;
using System.IO;
using Verse;

namespace RimWorld
{
	public static class LastPlayedVersion
	{
		private static bool initialized = false;

		private static Version lastPlayedVersionInt = null;

		public static Version Version
		{
			get
			{
				LastPlayedVersion.InitializeIfNeeded();
				return LastPlayedVersion.lastPlayedVersionInt;
			}
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static LastPlayedVersion()
		{
		}
	}
}
