using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FA5 RID: 4005
	public static class Prefs
	{
		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x0600609A RID: 24730 RVA: 0x0030DB10 File Offset: 0x0030BF10
		// (set) Token: 0x0600609B RID: 24731 RVA: 0x0030DB2F File Offset: 0x0030BF2F
		public static float VolumeGame
		{
			get
			{
				return Prefs.data.volumeGame;
			}
			set
			{
				Prefs.data.volumeGame = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x0600609C RID: 24732 RVA: 0x0030DB44 File Offset: 0x0030BF44
		// (set) Token: 0x0600609D RID: 24733 RVA: 0x0030DB63 File Offset: 0x0030BF63
		public static float VolumeMusic
		{
			get
			{
				return Prefs.data.volumeMusic;
			}
			set
			{
				Prefs.data.volumeMusic = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x0600609E RID: 24734 RVA: 0x0030DB78 File Offset: 0x0030BF78
		// (set) Token: 0x0600609F RID: 24735 RVA: 0x0030DB97 File Offset: 0x0030BF97
		public static float VolumeAmbient
		{
			get
			{
				return Prefs.data.volumeAmbient;
			}
			set
			{
				Prefs.data.volumeAmbient = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060060A0 RID: 24736 RVA: 0x0030DBAC File Offset: 0x0030BFAC
		// (set) Token: 0x060060A1 RID: 24737 RVA: 0x0030DBCB File Offset: 0x0030BFCB
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.data.adaptiveTrainingEnabled;
			}
			set
			{
				Prefs.data.adaptiveTrainingEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x060060A2 RID: 24738 RVA: 0x0030DBE0 File Offset: 0x0030BFE0
		// (set) Token: 0x060060A3 RID: 24739 RVA: 0x0030DBFF File Offset: 0x0030BFFF
		public static bool EdgeScreenScroll
		{
			get
			{
				return Prefs.data.edgeScreenScroll;
			}
			set
			{
				Prefs.data.edgeScreenScroll = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060060A4 RID: 24740 RVA: 0x0030DC14 File Offset: 0x0030C014
		// (set) Token: 0x060060A5 RID: 24741 RVA: 0x0030DC33 File Offset: 0x0030C033
		public static bool RunInBackground
		{
			get
			{
				return Prefs.data.runInBackground;
			}
			set
			{
				Prefs.data.runInBackground = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x060060A6 RID: 24742 RVA: 0x0030DC48 File Offset: 0x0030C048
		// (set) Token: 0x060060A7 RID: 24743 RVA: 0x0030DC67 File Offset: 0x0030C067
		public static TemperatureDisplayMode TemperatureMode
		{
			get
			{
				return Prefs.data.temperatureMode;
			}
			set
			{
				Prefs.data.temperatureMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x060060A8 RID: 24744 RVA: 0x0030DC7C File Offset: 0x0030C07C
		// (set) Token: 0x060060A9 RID: 24745 RVA: 0x0030DC9B File Offset: 0x0030C09B
		public static float AutosaveIntervalDays
		{
			get
			{
				return Prefs.data.autosaveIntervalDays;
			}
			set
			{
				Prefs.data.autosaveIntervalDays = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x060060AA RID: 24746 RVA: 0x0030DCB0 File Offset: 0x0030C0B0
		// (set) Token: 0x060060AB RID: 24747 RVA: 0x0030DCCF File Offset: 0x0030C0CF
		public static bool CustomCursorEnabled
		{
			get
			{
				return Prefs.data.customCursorEnabled;
			}
			set
			{
				Prefs.data.customCursorEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x060060AC RID: 24748 RVA: 0x0030DCE4 File Offset: 0x0030C0E4
		// (set) Token: 0x060060AD RID: 24749 RVA: 0x0030DD03 File Offset: 0x0030C103
		public static AnimalNameDisplayMode AnimalNameMode
		{
			get
			{
				return Prefs.data.animalNameMode;
			}
			set
			{
				Prefs.data.animalNameMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x060060AE RID: 24750 RVA: 0x0030DD18 File Offset: 0x0030C118
		// (set) Token: 0x060060AF RID: 24751 RVA: 0x0030DD48 File Offset: 0x0030C148
		public static bool DevMode
		{
			get
			{
				return Prefs.data == null || Prefs.data.devMode;
			}
			set
			{
				Prefs.data.devMode = value;
				if (!Prefs.data.devMode)
				{
					Prefs.data.logVerbose = false;
					Prefs.data.resetModsConfigOnCrash = true;
					DebugSettings.godMode = false;
				}
				Prefs.Apply();
			}
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x060060B0 RID: 24752 RVA: 0x0030DD88 File Offset: 0x0030C188
		// (set) Token: 0x060060B1 RID: 24753 RVA: 0x0030DDB7 File Offset: 0x0030C1B7
		public static bool ResetModsConfigOnCrash
		{
			get
			{
				return Prefs.data == null || Prefs.data.resetModsConfigOnCrash;
			}
			set
			{
				Prefs.data.resetModsConfigOnCrash = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x060060B2 RID: 24754 RVA: 0x0030DDCC File Offset: 0x0030C1CC
		// (set) Token: 0x060060B3 RID: 24755 RVA: 0x0030DDEB File Offset: 0x0030C1EB
		public static List<string> PreferredNames
		{
			get
			{
				return Prefs.data.preferredNames;
			}
			set
			{
				Prefs.data.preferredNames = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x060060B4 RID: 24756 RVA: 0x0030DE00 File Offset: 0x0030C200
		// (set) Token: 0x060060B5 RID: 24757 RVA: 0x0030DE1F File Offset: 0x0030C21F
		public static string LangFolderName
		{
			get
			{
				return Prefs.data.langFolderName;
			}
			set
			{
				Prefs.data.langFolderName = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x060060B6 RID: 24758 RVA: 0x0030DE34 File Offset: 0x0030C234
		// (set) Token: 0x060060B7 RID: 24759 RVA: 0x0030DE53 File Offset: 0x0030C253
		public static bool LogVerbose
		{
			get
			{
				return Prefs.data.logVerbose;
			}
			set
			{
				Prefs.data.logVerbose = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x060060B8 RID: 24760 RVA: 0x0030DE68 File Offset: 0x0030C268
		// (set) Token: 0x060060B9 RID: 24761 RVA: 0x0030DE97 File Offset: 0x0030C297
		public static bool PauseOnError
		{
			get
			{
				return Prefs.data != null && Prefs.data.pauseOnError;
			}
			set
			{
				Prefs.data.pauseOnError = value;
			}
		}

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x060060BA RID: 24762 RVA: 0x0030DEA8 File Offset: 0x0030C2A8
		// (set) Token: 0x060060BB RID: 24763 RVA: 0x0030DEC7 File Offset: 0x0030C2C7
		public static bool PauseOnLoad
		{
			get
			{
				return Prefs.data.pauseOnLoad;
			}
			set
			{
				Prefs.data.pauseOnLoad = value;
			}
		}

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x060060BC RID: 24764 RVA: 0x0030DED8 File Offset: 0x0030C2D8
		// (set) Token: 0x060060BD RID: 24765 RVA: 0x0030DEF7 File Offset: 0x0030C2F7
		public static bool PauseOnUrgentLetter
		{
			get
			{
				return Prefs.data.pauseOnUrgentLetter;
			}
			set
			{
				Prefs.data.pauseOnUrgentLetter = value;
			}
		}

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x060060BE RID: 24766 RVA: 0x0030DF08 File Offset: 0x0030C308
		// (set) Token: 0x060060BF RID: 24767 RVA: 0x0030DF27 File Offset: 0x0030C327
		public static bool ShowRealtimeClock
		{
			get
			{
				return Prefs.data.showRealtimeClock;
			}
			set
			{
				Prefs.data.showRealtimeClock = value;
			}
		}

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x060060C0 RID: 24768 RVA: 0x0030DF38 File Offset: 0x0030C338
		// (set) Token: 0x060060C1 RID: 24769 RVA: 0x0030DF57 File Offset: 0x0030C357
		public static bool TestMapSizes
		{
			get
			{
				return Prefs.data.testMapSizes;
			}
			set
			{
				Prefs.data.testMapSizes = value;
			}
		}

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x060060C2 RID: 24770 RVA: 0x0030DF68 File Offset: 0x0030C368
		// (set) Token: 0x060060C3 RID: 24771 RVA: 0x0030DF87 File Offset: 0x0030C387
		public static int MaxNumberOfPlayerHomes
		{
			get
			{
				return Prefs.data.maxNumberOfPlayerHomes;
			}
			set
			{
				Prefs.data.maxNumberOfPlayerHomes = value;
			}
		}

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x060060C4 RID: 24772 RVA: 0x0030DF98 File Offset: 0x0030C398
		// (set) Token: 0x060060C5 RID: 24773 RVA: 0x0030DFB7 File Offset: 0x0030C3B7
		public static bool PlantWindSway
		{
			get
			{
				return Prefs.data.plantWindSway;
			}
			set
			{
				Prefs.data.plantWindSway = value;
			}
		}

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x060060C6 RID: 24774 RVA: 0x0030DFC8 File Offset: 0x0030C3C8
		// (set) Token: 0x060060C7 RID: 24775 RVA: 0x0030DFE7 File Offset: 0x0030C3E7
		public static bool ResourceReadoutCategorized
		{
			get
			{
				return Prefs.data.resourceReadoutCategorized;
			}
			set
			{
				if (value != Prefs.data.resourceReadoutCategorized)
				{
					Prefs.data.resourceReadoutCategorized = value;
					Prefs.Save();
				}
			}
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x060060C8 RID: 24776 RVA: 0x0030E010 File Offset: 0x0030C410
		// (set) Token: 0x060060C9 RID: 24777 RVA: 0x0030E02F File Offset: 0x0030C42F
		public static float UIScale
		{
			get
			{
				return Prefs.data.uiScale;
			}
			set
			{
				Prefs.data.uiScale = value;
			}
		}

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x060060CA RID: 24778 RVA: 0x0030E040 File Offset: 0x0030C440
		// (set) Token: 0x060060CB RID: 24779 RVA: 0x0030E05F File Offset: 0x0030C45F
		public static bool HatsOnlyOnMap
		{
			get
			{
				return Prefs.data.hatsOnlyOnMap;
			}
			set
			{
				Prefs.data.hatsOnlyOnMap = value;
				Prefs.Apply();
			}
		}

		// Token: 0x060060CC RID: 24780 RVA: 0x0030E074 File Offset: 0x0030C474
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.PrefsFilePath).Exists;
			Prefs.data = new PrefsData();
			Prefs.data = DirectXmlLoader.ItemFromXmlFile<PrefsData>(GenFilePaths.PrefsFilePath, true);
			if (flag)
			{
				Prefs.data.langFolderName = LanguageDatabase.SystemLanguageFolderName();
				if (UnityData.isDebugBuild && !DevModePermanentlyDisabledUtility.Disabled)
				{
					Prefs.DevMode = true;
				}
			}
			if (DevModePermanentlyDisabledUtility.Disabled)
			{
				Prefs.DevMode = false;
			}
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x0030E0F0 File Offset: 0x0030C4F0
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(Prefs.data, typeof(PrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.PrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(new object[]
				{
					GenFilePaths.PrefsFilePath,
					ex.ToString()
				}));
				Log.Error("Exception saving prefs: " + ex, false);
			}
		}

		// Token: 0x060060CE RID: 24782 RVA: 0x0030E184 File Offset: 0x0030C584
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x060060CF RID: 24783 RVA: 0x0030E194 File Offset: 0x0030C594
		public static NameTriple RandomPreferredName()
		{
			string rawName;
			NameTriple result;
			if ((from name in Prefs.PreferredNames
			where !name.NullOrEmpty()
			select name).TryRandomElement(out rawName))
			{
				result = NameTriple.FromString(rawName);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04003F45 RID: 16197
		private static PrefsData data;
	}
}
