using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FA9 RID: 4009
	public static class Prefs
	{
		// Token: 0x04003F61 RID: 16225
		private static PrefsData data;

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x060060CB RID: 24779 RVA: 0x00310554 File Offset: 0x0030E954
		// (set) Token: 0x060060CC RID: 24780 RVA: 0x00310573 File Offset: 0x0030E973
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

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060060CD RID: 24781 RVA: 0x00310588 File Offset: 0x0030E988
		// (set) Token: 0x060060CE RID: 24782 RVA: 0x003105A7 File Offset: 0x0030E9A7
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

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x060060CF RID: 24783 RVA: 0x003105BC File Offset: 0x0030E9BC
		// (set) Token: 0x060060D0 RID: 24784 RVA: 0x003105DB File Offset: 0x0030E9DB
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

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060060D1 RID: 24785 RVA: 0x003105F0 File Offset: 0x0030E9F0
		// (set) Token: 0x060060D2 RID: 24786 RVA: 0x0031060F File Offset: 0x0030EA0F
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

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x060060D3 RID: 24787 RVA: 0x00310624 File Offset: 0x0030EA24
		// (set) Token: 0x060060D4 RID: 24788 RVA: 0x00310643 File Offset: 0x0030EA43
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

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x060060D5 RID: 24789 RVA: 0x00310658 File Offset: 0x0030EA58
		// (set) Token: 0x060060D6 RID: 24790 RVA: 0x00310677 File Offset: 0x0030EA77
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

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x060060D7 RID: 24791 RVA: 0x0031068C File Offset: 0x0030EA8C
		// (set) Token: 0x060060D8 RID: 24792 RVA: 0x003106AB File Offset: 0x0030EAAB
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

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x060060D9 RID: 24793 RVA: 0x003106C0 File Offset: 0x0030EAC0
		// (set) Token: 0x060060DA RID: 24794 RVA: 0x003106DF File Offset: 0x0030EADF
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

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x060060DB RID: 24795 RVA: 0x003106F4 File Offset: 0x0030EAF4
		// (set) Token: 0x060060DC RID: 24796 RVA: 0x00310713 File Offset: 0x0030EB13
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

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x060060DD RID: 24797 RVA: 0x00310728 File Offset: 0x0030EB28
		// (set) Token: 0x060060DE RID: 24798 RVA: 0x00310747 File Offset: 0x0030EB47
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

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x060060DF RID: 24799 RVA: 0x0031075C File Offset: 0x0030EB5C
		// (set) Token: 0x060060E0 RID: 24800 RVA: 0x0031078C File Offset: 0x0030EB8C
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

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x060060E1 RID: 24801 RVA: 0x003107CC File Offset: 0x0030EBCC
		// (set) Token: 0x060060E2 RID: 24802 RVA: 0x003107FB File Offset: 0x0030EBFB
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

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x060060E3 RID: 24803 RVA: 0x00310810 File Offset: 0x0030EC10
		// (set) Token: 0x060060E4 RID: 24804 RVA: 0x0031082F File Offset: 0x0030EC2F
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

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x060060E5 RID: 24805 RVA: 0x00310844 File Offset: 0x0030EC44
		// (set) Token: 0x060060E6 RID: 24806 RVA: 0x00310863 File Offset: 0x0030EC63
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

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x060060E7 RID: 24807 RVA: 0x00310878 File Offset: 0x0030EC78
		// (set) Token: 0x060060E8 RID: 24808 RVA: 0x00310897 File Offset: 0x0030EC97
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

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x060060E9 RID: 24809 RVA: 0x003108AC File Offset: 0x0030ECAC
		// (set) Token: 0x060060EA RID: 24810 RVA: 0x003108DB File Offset: 0x0030ECDB
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

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x060060EB RID: 24811 RVA: 0x003108EC File Offset: 0x0030ECEC
		// (set) Token: 0x060060EC RID: 24812 RVA: 0x0031090B File Offset: 0x0030ED0B
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

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x060060ED RID: 24813 RVA: 0x0031091C File Offset: 0x0030ED1C
		// (set) Token: 0x060060EE RID: 24814 RVA: 0x0031093B File Offset: 0x0030ED3B
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

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x060060EF RID: 24815 RVA: 0x0031094C File Offset: 0x0030ED4C
		// (set) Token: 0x060060F0 RID: 24816 RVA: 0x0031096B File Offset: 0x0030ED6B
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

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x060060F1 RID: 24817 RVA: 0x0031097C File Offset: 0x0030ED7C
		// (set) Token: 0x060060F2 RID: 24818 RVA: 0x0031099B File Offset: 0x0030ED9B
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

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x060060F3 RID: 24819 RVA: 0x003109AC File Offset: 0x0030EDAC
		// (set) Token: 0x060060F4 RID: 24820 RVA: 0x003109CB File Offset: 0x0030EDCB
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

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x060060F5 RID: 24821 RVA: 0x003109DC File Offset: 0x0030EDDC
		// (set) Token: 0x060060F6 RID: 24822 RVA: 0x003109FB File Offset: 0x0030EDFB
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

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x060060F7 RID: 24823 RVA: 0x00310A0C File Offset: 0x0030EE0C
		// (set) Token: 0x060060F8 RID: 24824 RVA: 0x00310A2B File Offset: 0x0030EE2B
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

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x060060F9 RID: 24825 RVA: 0x00310A54 File Offset: 0x0030EE54
		// (set) Token: 0x060060FA RID: 24826 RVA: 0x00310A73 File Offset: 0x0030EE73
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

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x060060FB RID: 24827 RVA: 0x00310A84 File Offset: 0x0030EE84
		// (set) Token: 0x060060FC RID: 24828 RVA: 0x00310AA3 File Offset: 0x0030EEA3
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

		// Token: 0x060060FD RID: 24829 RVA: 0x00310AB8 File Offset: 0x0030EEB8
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

		// Token: 0x060060FE RID: 24830 RVA: 0x00310B34 File Offset: 0x0030EF34
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

		// Token: 0x060060FF RID: 24831 RVA: 0x00310BC8 File Offset: 0x0030EFC8
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x06006100 RID: 24832 RVA: 0x00310BD8 File Offset: 0x0030EFD8
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
