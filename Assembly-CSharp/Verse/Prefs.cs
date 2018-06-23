using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FA4 RID: 4004
	public static class Prefs
	{
		// Token: 0x04003F56 RID: 16214
		private static PrefsData data;

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060060C1 RID: 24769 RVA: 0x0030FC90 File Offset: 0x0030E090
		// (set) Token: 0x060060C2 RID: 24770 RVA: 0x0030FCAF File Offset: 0x0030E0AF
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

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x060060C3 RID: 24771 RVA: 0x0030FCC4 File Offset: 0x0030E0C4
		// (set) Token: 0x060060C4 RID: 24772 RVA: 0x0030FCE3 File Offset: 0x0030E0E3
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

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060060C5 RID: 24773 RVA: 0x0030FCF8 File Offset: 0x0030E0F8
		// (set) Token: 0x060060C6 RID: 24774 RVA: 0x0030FD17 File Offset: 0x0030E117
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

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x060060C7 RID: 24775 RVA: 0x0030FD2C File Offset: 0x0030E12C
		// (set) Token: 0x060060C8 RID: 24776 RVA: 0x0030FD4B File Offset: 0x0030E14B
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

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x060060C9 RID: 24777 RVA: 0x0030FD60 File Offset: 0x0030E160
		// (set) Token: 0x060060CA RID: 24778 RVA: 0x0030FD7F File Offset: 0x0030E17F
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

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x060060CB RID: 24779 RVA: 0x0030FD94 File Offset: 0x0030E194
		// (set) Token: 0x060060CC RID: 24780 RVA: 0x0030FDB3 File Offset: 0x0030E1B3
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

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x060060CD RID: 24781 RVA: 0x0030FDC8 File Offset: 0x0030E1C8
		// (set) Token: 0x060060CE RID: 24782 RVA: 0x0030FDE7 File Offset: 0x0030E1E7
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

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x060060CF RID: 24783 RVA: 0x0030FDFC File Offset: 0x0030E1FC
		// (set) Token: 0x060060D0 RID: 24784 RVA: 0x0030FE1B File Offset: 0x0030E21B
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

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x060060D1 RID: 24785 RVA: 0x0030FE30 File Offset: 0x0030E230
		// (set) Token: 0x060060D2 RID: 24786 RVA: 0x0030FE4F File Offset: 0x0030E24F
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

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x060060D3 RID: 24787 RVA: 0x0030FE64 File Offset: 0x0030E264
		// (set) Token: 0x060060D4 RID: 24788 RVA: 0x0030FE83 File Offset: 0x0030E283
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

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x060060D5 RID: 24789 RVA: 0x0030FE98 File Offset: 0x0030E298
		// (set) Token: 0x060060D6 RID: 24790 RVA: 0x0030FEC8 File Offset: 0x0030E2C8
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

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x060060D7 RID: 24791 RVA: 0x0030FF08 File Offset: 0x0030E308
		// (set) Token: 0x060060D8 RID: 24792 RVA: 0x0030FF37 File Offset: 0x0030E337
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

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x060060D9 RID: 24793 RVA: 0x0030FF4C File Offset: 0x0030E34C
		// (set) Token: 0x060060DA RID: 24794 RVA: 0x0030FF6B File Offset: 0x0030E36B
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

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x060060DB RID: 24795 RVA: 0x0030FF80 File Offset: 0x0030E380
		// (set) Token: 0x060060DC RID: 24796 RVA: 0x0030FF9F File Offset: 0x0030E39F
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

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x060060DD RID: 24797 RVA: 0x0030FFB4 File Offset: 0x0030E3B4
		// (set) Token: 0x060060DE RID: 24798 RVA: 0x0030FFD3 File Offset: 0x0030E3D3
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

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x060060DF RID: 24799 RVA: 0x0030FFE8 File Offset: 0x0030E3E8
		// (set) Token: 0x060060E0 RID: 24800 RVA: 0x00310017 File Offset: 0x0030E417
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

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x060060E1 RID: 24801 RVA: 0x00310028 File Offset: 0x0030E428
		// (set) Token: 0x060060E2 RID: 24802 RVA: 0x00310047 File Offset: 0x0030E447
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

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x060060E3 RID: 24803 RVA: 0x00310058 File Offset: 0x0030E458
		// (set) Token: 0x060060E4 RID: 24804 RVA: 0x00310077 File Offset: 0x0030E477
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

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x060060E5 RID: 24805 RVA: 0x00310088 File Offset: 0x0030E488
		// (set) Token: 0x060060E6 RID: 24806 RVA: 0x003100A7 File Offset: 0x0030E4A7
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

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x060060E7 RID: 24807 RVA: 0x003100B8 File Offset: 0x0030E4B8
		// (set) Token: 0x060060E8 RID: 24808 RVA: 0x003100D7 File Offset: 0x0030E4D7
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

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x060060E9 RID: 24809 RVA: 0x003100E8 File Offset: 0x0030E4E8
		// (set) Token: 0x060060EA RID: 24810 RVA: 0x00310107 File Offset: 0x0030E507
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

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x060060EB RID: 24811 RVA: 0x00310118 File Offset: 0x0030E518
		// (set) Token: 0x060060EC RID: 24812 RVA: 0x00310137 File Offset: 0x0030E537
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

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x060060ED RID: 24813 RVA: 0x00310148 File Offset: 0x0030E548
		// (set) Token: 0x060060EE RID: 24814 RVA: 0x00310167 File Offset: 0x0030E567
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

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x060060EF RID: 24815 RVA: 0x00310190 File Offset: 0x0030E590
		// (set) Token: 0x060060F0 RID: 24816 RVA: 0x003101AF File Offset: 0x0030E5AF
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

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x060060F1 RID: 24817 RVA: 0x003101C0 File Offset: 0x0030E5C0
		// (set) Token: 0x060060F2 RID: 24818 RVA: 0x003101DF File Offset: 0x0030E5DF
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

		// Token: 0x060060F3 RID: 24819 RVA: 0x003101F4 File Offset: 0x0030E5F4
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

		// Token: 0x060060F4 RID: 24820 RVA: 0x00310270 File Offset: 0x0030E670
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

		// Token: 0x060060F5 RID: 24821 RVA: 0x00310304 File Offset: 0x0030E704
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x060060F6 RID: 24822 RVA: 0x00310314 File Offset: 0x0030E714
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
	}
}
