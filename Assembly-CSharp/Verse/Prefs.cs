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
		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x06006098 RID: 24728 RVA: 0x0030DBEC File Offset: 0x0030BFEC
		// (set) Token: 0x06006099 RID: 24729 RVA: 0x0030DC0B File Offset: 0x0030C00B
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

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x0600609A RID: 24730 RVA: 0x0030DC20 File Offset: 0x0030C020
		// (set) Token: 0x0600609B RID: 24731 RVA: 0x0030DC3F File Offset: 0x0030C03F
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

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x0600609C RID: 24732 RVA: 0x0030DC54 File Offset: 0x0030C054
		// (set) Token: 0x0600609D RID: 24733 RVA: 0x0030DC73 File Offset: 0x0030C073
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

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x0600609E RID: 24734 RVA: 0x0030DC88 File Offset: 0x0030C088
		// (set) Token: 0x0600609F RID: 24735 RVA: 0x0030DCA7 File Offset: 0x0030C0A7
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

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060060A0 RID: 24736 RVA: 0x0030DCBC File Offset: 0x0030C0BC
		// (set) Token: 0x060060A1 RID: 24737 RVA: 0x0030DCDB File Offset: 0x0030C0DB
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

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x060060A2 RID: 24738 RVA: 0x0030DCF0 File Offset: 0x0030C0F0
		// (set) Token: 0x060060A3 RID: 24739 RVA: 0x0030DD0F File Offset: 0x0030C10F
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

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060060A4 RID: 24740 RVA: 0x0030DD24 File Offset: 0x0030C124
		// (set) Token: 0x060060A5 RID: 24741 RVA: 0x0030DD43 File Offset: 0x0030C143
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

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x060060A6 RID: 24742 RVA: 0x0030DD58 File Offset: 0x0030C158
		// (set) Token: 0x060060A7 RID: 24743 RVA: 0x0030DD77 File Offset: 0x0030C177
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

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x060060A8 RID: 24744 RVA: 0x0030DD8C File Offset: 0x0030C18C
		// (set) Token: 0x060060A9 RID: 24745 RVA: 0x0030DDAB File Offset: 0x0030C1AB
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

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x060060AA RID: 24746 RVA: 0x0030DDC0 File Offset: 0x0030C1C0
		// (set) Token: 0x060060AB RID: 24747 RVA: 0x0030DDDF File Offset: 0x0030C1DF
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

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x060060AC RID: 24748 RVA: 0x0030DDF4 File Offset: 0x0030C1F4
		// (set) Token: 0x060060AD RID: 24749 RVA: 0x0030DE24 File Offset: 0x0030C224
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

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x060060AE RID: 24750 RVA: 0x0030DE64 File Offset: 0x0030C264
		// (set) Token: 0x060060AF RID: 24751 RVA: 0x0030DE93 File Offset: 0x0030C293
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

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x060060B0 RID: 24752 RVA: 0x0030DEA8 File Offset: 0x0030C2A8
		// (set) Token: 0x060060B1 RID: 24753 RVA: 0x0030DEC7 File Offset: 0x0030C2C7
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

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x060060B2 RID: 24754 RVA: 0x0030DEDC File Offset: 0x0030C2DC
		// (set) Token: 0x060060B3 RID: 24755 RVA: 0x0030DEFB File Offset: 0x0030C2FB
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

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x060060B4 RID: 24756 RVA: 0x0030DF10 File Offset: 0x0030C310
		// (set) Token: 0x060060B5 RID: 24757 RVA: 0x0030DF2F File Offset: 0x0030C32F
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

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x060060B6 RID: 24758 RVA: 0x0030DF44 File Offset: 0x0030C344
		// (set) Token: 0x060060B7 RID: 24759 RVA: 0x0030DF73 File Offset: 0x0030C373
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

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x060060B8 RID: 24760 RVA: 0x0030DF84 File Offset: 0x0030C384
		// (set) Token: 0x060060B9 RID: 24761 RVA: 0x0030DFA3 File Offset: 0x0030C3A3
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

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x060060BA RID: 24762 RVA: 0x0030DFB4 File Offset: 0x0030C3B4
		// (set) Token: 0x060060BB RID: 24763 RVA: 0x0030DFD3 File Offset: 0x0030C3D3
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

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x060060BC RID: 24764 RVA: 0x0030DFE4 File Offset: 0x0030C3E4
		// (set) Token: 0x060060BD RID: 24765 RVA: 0x0030E003 File Offset: 0x0030C403
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

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x060060BE RID: 24766 RVA: 0x0030E014 File Offset: 0x0030C414
		// (set) Token: 0x060060BF RID: 24767 RVA: 0x0030E033 File Offset: 0x0030C433
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

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x060060C0 RID: 24768 RVA: 0x0030E044 File Offset: 0x0030C444
		// (set) Token: 0x060060C1 RID: 24769 RVA: 0x0030E063 File Offset: 0x0030C463
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

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x060060C2 RID: 24770 RVA: 0x0030E074 File Offset: 0x0030C474
		// (set) Token: 0x060060C3 RID: 24771 RVA: 0x0030E093 File Offset: 0x0030C493
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

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x060060C4 RID: 24772 RVA: 0x0030E0A4 File Offset: 0x0030C4A4
		// (set) Token: 0x060060C5 RID: 24773 RVA: 0x0030E0C3 File Offset: 0x0030C4C3
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

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x060060C6 RID: 24774 RVA: 0x0030E0EC File Offset: 0x0030C4EC
		// (set) Token: 0x060060C7 RID: 24775 RVA: 0x0030E10B File Offset: 0x0030C50B
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

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x060060C8 RID: 24776 RVA: 0x0030E11C File Offset: 0x0030C51C
		// (set) Token: 0x060060C9 RID: 24777 RVA: 0x0030E13B File Offset: 0x0030C53B
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

		// Token: 0x060060CA RID: 24778 RVA: 0x0030E150 File Offset: 0x0030C550
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

		// Token: 0x060060CB RID: 24779 RVA: 0x0030E1CC File Offset: 0x0030C5CC
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

		// Token: 0x060060CC RID: 24780 RVA: 0x0030E260 File Offset: 0x0030C660
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x0030E270 File Offset: 0x0030C670
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

		// Token: 0x04003F44 RID: 16196
		private static PrefsData data;
	}
}
