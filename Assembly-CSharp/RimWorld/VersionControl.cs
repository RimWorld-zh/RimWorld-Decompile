using System;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020009D2 RID: 2514
	public static class VersionControl
	{
		// Token: 0x0600384D RID: 14413 RVA: 0x001DFF14 File Offset: 0x001DE314
		static VersionControl()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			DateTime dateTime = new DateTime(2000, 1, 1);
			VersionControl.buildDate = dateTime.AddDays((double)version.Build);
			int build = version.Build - 4805;
			int revision = version.Revision * 2 / 60;
			VersionControl.version = new Version(version.Major, version.Minor, build, revision);
			VersionControl.versionStringWithRev = string.Concat(new object[]
			{
				VersionControl.version.Major,
				".",
				VersionControl.version.Minor,
				".",
				VersionControl.version.Build,
				" rev",
				VersionControl.version.Revision
			});
			VersionControl.versionString = string.Concat(new object[]
			{
				VersionControl.version.Major,
				".",
				VersionControl.version.Minor,
				".",
				VersionControl.version.Build
			});
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x0600384E RID: 14414 RVA: 0x001E004C File Offset: 0x001DE44C
		public static Version CurrentVersion
		{
			get
			{
				return VersionControl.version;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x0600384F RID: 14415 RVA: 0x001E0068 File Offset: 0x001DE468
		public static string CurrentVersionString
		{
			get
			{
				return VersionControl.versionString;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003850 RID: 14416 RVA: 0x001E0084 File Offset: 0x001DE484
		public static string CurrentVersionStringWithRev
		{
			get
			{
				return VersionControl.versionStringWithRev;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06003851 RID: 14417 RVA: 0x001E00A0 File Offset: 0x001DE4A0
		public static int CurrentMajor
		{
			get
			{
				return VersionControl.version.Major;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06003852 RID: 14418 RVA: 0x001E00C0 File Offset: 0x001DE4C0
		public static int CurrentMinor
		{
			get
			{
				return VersionControl.version.Minor;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06003853 RID: 14419 RVA: 0x001E00E0 File Offset: 0x001DE4E0
		public static int CurrentBuild
		{
			get
			{
				return VersionControl.version.Build;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x001E0100 File Offset: 0x001DE500
		public static int CurrentRevision
		{
			get
			{
				return VersionControl.version.Revision;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003855 RID: 14421 RVA: 0x001E0120 File Offset: 0x001DE520
		public static DateTime CurrentBuildDate
		{
			get
			{
				return VersionControl.buildDate;
			}
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x001E013C File Offset: 0x001DE53C
		public static void DrawInfoInCorner()
		{
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			string text = "VersionIndicator".Translate(new object[]
			{
				VersionControl.versionString
			});
			if (UnityData.isDebugBuild)
			{
				text = text + " (" + "DevelopmentBuildLower".Translate() + ")";
			}
			text = text + "\n" + "CompiledOn".Translate(new object[]
			{
				VersionControl.buildDate.ToString("MMM d yyyy")
			});
			if (SteamManager.Initialized)
			{
				text = text + "\n" + "LoggedIntoSteamAs".Translate(new object[]
				{
					SteamUtility.SteamPersonaName
				});
			}
			Rect rect = new Rect(10f, 10f, 330f, Text.CalcHeight(text, 330f));
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			LatestVersionGetter component = Current.Root.gameObject.GetComponent<LatestVersionGetter>();
			Rect rect2 = new Rect(10f, rect.yMax - 5f, 330f, 999f);
			component.DrawAt(rect2);
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x001E0274 File Offset: 0x001DE674
		public static void LogVersionNumber()
		{
			Log.Message("RimWorld " + VersionControl.versionStringWithRev, false);
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x001E028C File Offset: 0x001DE68C
		public static bool IsWellFormattedVersionString(string str)
		{
			string[] array = str.Split(new char[]
			{
				'.'
			});
			bool result;
			if (array.Length != 3)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					int num;
					if (!int.TryParse(array[i], out num))
					{
						return false;
					}
					if (num < 0)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x001E02FC File Offset: 0x001DE6FC
		public static int BuildFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length < 3 || !int.TryParse(array[2], out result))
			{
				Log.Warning("Could not get build from version string " + str, false);
			}
			return result;
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x001E0358 File Offset: 0x001DE758
		public static int MinorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length < 2 || !int.TryParse(array[1], out result))
			{
				Log.Warning("Could not get minor version from version string " + str, false);
			}
			return result;
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x001E03B4 File Offset: 0x001DE7B4
		public static int MajorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			if (!int.TryParse(str.Split(new char[]
			{
				'.'
			})[0], out result))
			{
				Log.Warning("Could not get major version from version string " + str, false);
			}
			return result;
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x001E0404 File Offset: 0x001DE804
		public static string VersionStringWithoutRev(string str)
		{
			return str.Split(new char[]
			{
				' '
			})[0];
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x001E042C File Offset: 0x001DE82C
		public static Version VersionFromString(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException("str");
			}
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length > 3)
			{
				throw new ArgumentException("str");
			}
			int major = 0;
			int minor = 0;
			int build = 0;
			for (int i = 0; i < 3; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					throw new ArgumentException("str");
				}
				if (num < 0)
				{
					throw new ArgumentException("str");
				}
				if (i != 0)
				{
					if (i != 1)
					{
						if (i == 2)
						{
							build = num;
						}
					}
					else
					{
						minor = num;
					}
				}
				else
				{
					major = num;
				}
			}
			return new Version(major, minor, build);
		}

		// Token: 0x04002407 RID: 9223
		private static Version version;

		// Token: 0x04002408 RID: 9224
		private static string versionString;

		// Token: 0x04002409 RID: 9225
		private static string versionStringWithRev;

		// Token: 0x0400240A RID: 9226
		private static DateTime buildDate;
	}
}
