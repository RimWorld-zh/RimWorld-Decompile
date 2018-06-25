using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F41 RID: 3905
	public static class GenFilePaths
	{
		// Token: 0x04003E02 RID: 15874
		private static string saveDataPath = null;

		// Token: 0x04003E03 RID: 15875
		private static string coreModsFolderPath = null;

		// Token: 0x04003E04 RID: 15876
		public const string SoundsFolder = "Sounds/";

		// Token: 0x04003E05 RID: 15877
		public const string TexturesFolder = "Textures/";

		// Token: 0x04003E06 RID: 15878
		public const string StringsFolder = "Strings/";

		// Token: 0x04003E07 RID: 15879
		public const string DefsFolder = "Defs/";

		// Token: 0x04003E08 RID: 15880
		public const string PatchesFolder = "Patches/";

		// Token: 0x04003E09 RID: 15881
		public const string BackstoriesPath = "Backstories";

		// Token: 0x04003E0A RID: 15882
		public const string SavedGameExtension = ".rws";

		// Token: 0x04003E0B RID: 15883
		public const string ScenarioExtension = ".rsc";

		// Token: 0x04003E0C RID: 15884
		public const string ExternalHistoryFileExtension = ".rwh";

		// Token: 0x04003E0D RID: 15885
		private const string SaveDataFolderCommand = "savedatafolder";

		// Token: 0x04003E0E RID: 15886
		private static readonly string[] FilePathRaw = new string[]
		{
			"Ž",
			"ž",
			"Ÿ",
			"¡",
			"¢",
			"£",
			"¤",
			"¥",
			"¦",
			"§",
			"¨",
			"©",
			"ª",
			"À",
			"Á",
			"Â",
			"Ã",
			"Ä",
			"Å",
			"Æ",
			"Ç",
			"È",
			"É",
			"Ê",
			"Ë",
			"Ì",
			"Í",
			"Î",
			"Ï",
			"Ð",
			"Ñ",
			"Ò",
			"Ó",
			"Ô",
			"Õ",
			"Ö",
			"Ù",
			"Ú",
			"Û",
			"Ü",
			"Ý",
			"Þ",
			"ß",
			"à",
			"á",
			"â",
			"ã",
			"ä",
			"å",
			"æ",
			"ç",
			"è",
			"é",
			"ê",
			"ë",
			"ì",
			"í",
			"î",
			"ï",
			"ð",
			"ñ",
			"ò",
			"ó",
			"ô",
			"õ",
			"ö",
			"ù",
			"ú",
			"û",
			"ü",
			"ý",
			"þ",
			"ÿ"
		};

		// Token: 0x04003E0F RID: 15887
		private static readonly string[] FilePathSafe = new string[]
		{
			"%8E",
			"%9E",
			"%9F",
			"%A1",
			"%A2",
			"%A3",
			"%A4",
			"%A5",
			"%A6",
			"%A7",
			"%A8",
			"%A9",
			"%AA",
			"%C0",
			"%C1",
			"%C2",
			"%C3",
			"%C4",
			"%C5",
			"%C6",
			"%C7",
			"%C8",
			"%C9",
			"%CA",
			"%CB",
			"%CC",
			"%CD",
			"%CE",
			"%CF",
			"%D0",
			"%D1",
			"%D2",
			"%D3",
			"%D4",
			"%D5",
			"%D6",
			"%D9",
			"%DA",
			"%DB",
			"%DC",
			"%DD",
			"%DE",
			"%DF",
			"%E0",
			"%E1",
			"%E2",
			"%E3",
			"%E4",
			"%E5",
			"%E6",
			"%E7",
			"%E8",
			"%E9",
			"%EA",
			"%EB",
			"%EC",
			"%ED",
			"%EE",
			"%EF",
			"%F0",
			"%F1",
			"%F2",
			"%F3",
			"%F4",
			"%F5",
			"%F6",
			"%F9",
			"%FA",
			"%FB",
			"%FC",
			"%FD",
			"%FE",
			"%FF"
		};

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06005E20 RID: 24096 RVA: 0x002FE420 File Offset: 0x002FC820
		public static string SaveDataFolderPath
		{
			get
			{
				if (GenFilePaths.saveDataPath == null)
				{
					string text;
					if (GenCommandLine.TryGetCommandLineArg("savedatafolder", out text))
					{
						text.TrimEnd(new char[]
						{
							'\\',
							'/'
						});
						if (text == "")
						{
							text = "" + Path.DirectorySeparatorChar;
						}
						GenFilePaths.saveDataPath = text;
						Log.Message("Save data folder overridden to " + GenFilePaths.saveDataPath, false);
					}
					else
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
						if (UnityData.isEditor)
						{
							GenFilePaths.saveDataPath = Path.Combine(directoryInfo.Parent.ToString(), "SaveData");
						}
						else if (UnityData.platform == RuntimePlatform.OSXPlayer || UnityData.platform == RuntimePlatform.OSXEditor || UnityData.platform == RuntimePlatform.OSXDashboardPlayer)
						{
							DirectoryInfo parent = Directory.GetParent(UnityData.persistentDataPath);
							string path = Path.Combine(parent.ToString(), "RimWorld");
							if (!Directory.Exists(path))
							{
								Directory.CreateDirectory(path);
							}
							GenFilePaths.saveDataPath = path;
						}
						else
						{
							GenFilePaths.saveDataPath = Application.persistentDataPath;
						}
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(GenFilePaths.saveDataPath);
					if (!directoryInfo2.Exists)
					{
						directoryInfo2.Create();
					}
				}
				return GenFilePaths.saveDataPath;
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06005E21 RID: 24097 RVA: 0x002FE578 File Offset: 0x002FC978
		public static string ScenarioPreviewImagePath
		{
			get
			{
				string result;
				if (!UnityData.isEditor)
				{
					result = Path.Combine(GenFilePaths.ExecutableDir.FullName, "ScenarioPreview.jpg");
				}
				else
				{
					result = Path.Combine(Path.Combine(Path.Combine(GenFilePaths.ExecutableDir.FullName, "PlatformSpecific"), "All"), "ScenarioPreview.jpg");
				}
				return result;
			}
		}

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x06005E22 RID: 24098 RVA: 0x002FE5DC File Offset: 0x002FC9DC
		private static DirectoryInfo ExecutableDir
		{
			get
			{
				return new DirectoryInfo(UnityData.dataPath).Parent;
			}
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06005E23 RID: 24099 RVA: 0x002FE600 File Offset: 0x002FCA00
		public static string CoreModsFolderPath
		{
			get
			{
				if (GenFilePaths.coreModsFolderPath == null)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
					DirectoryInfo directoryInfo2;
					if (UnityData.isEditor)
					{
						directoryInfo2 = directoryInfo;
					}
					else
					{
						directoryInfo2 = directoryInfo.Parent;
					}
					GenFilePaths.coreModsFolderPath = Path.Combine(directoryInfo2.ToString(), "Mods");
					if (UnityData.isDebugBuild)
					{
						DirectoryInfo directoryInfo3 = new DirectoryInfo(GenFilePaths.coreModsFolderPath);
						if (!directoryInfo3.Exists)
						{
							GenFilePaths.coreModsFolderPath = Path.Combine(directoryInfo.Parent.Parent.ToString(), "RimWorld/Assets/Mods");
						}
					}
					DirectoryInfo directoryInfo4 = new DirectoryInfo(GenFilePaths.coreModsFolderPath);
					if (!directoryInfo4.Exists)
					{
						directoryInfo4.Create();
					}
				}
				return GenFilePaths.coreModsFolderPath;
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06005E24 RID: 24100 RVA: 0x002FE6C0 File Offset: 0x002FCAC0
		public static string ConfigFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Config");
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06005E25 RID: 24101 RVA: 0x002FE6E0 File Offset: 0x002FCAE0
		private static string SavedGamesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Saves");
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06005E26 RID: 24102 RVA: 0x002FE700 File Offset: 0x002FCB00
		private static string ScenariosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Scenarios");
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06005E27 RID: 24103 RVA: 0x002FE720 File Offset: 0x002FCB20
		private static string ExternalHistoryFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("External");
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x06005E28 RID: 24104 RVA: 0x002FE740 File Offset: 0x002FCB40
		public static string ScreenshotFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Screenshots");
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06005E29 RID: 24105 RVA: 0x002FE760 File Offset: 0x002FCB60
		public static string DevOutputFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("DevOutput");
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06005E2A RID: 24106 RVA: 0x002FE780 File Offset: 0x002FCB80
		public static string ModsConfigFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "ModsConfig.xml");
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06005E2B RID: 24107 RVA: 0x002FE7A4 File Offset: 0x002FCBA4
		public static string ConceptKnowledgeFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Knowledge.xml");
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x06005E2C RID: 24108 RVA: 0x002FE7C8 File Offset: 0x002FCBC8
		public static string PrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Prefs.xml");
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06005E2D RID: 24109 RVA: 0x002FE7EC File Offset: 0x002FCBEC
		public static string KeyPrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "KeyPrefs.xml");
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06005E2E RID: 24110 RVA: 0x002FE810 File Offset: 0x002FCC10
		public static string LastPlayedVersionFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "LastPlayedVersion.txt");
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06005E2F RID: 24111 RVA: 0x002FE834 File Offset: 0x002FCC34
		public static string DevModePermanentlyDisabledFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "DevModeDisabled");
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06005E30 RID: 24112 RVA: 0x002FE858 File Offset: 0x002FCC58
		public static string BackstoryOutputFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.DevOutputFolderPath, "Fresh_Backstories.xml");
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06005E31 RID: 24113 RVA: 0x002FE87C File Offset: 0x002FCC7C
		public static string TempFolderPath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06005E32 RID: 24114 RVA: 0x002FE898 File Offset: 0x002FCC98
		public static IEnumerable<FileInfo> AllSavedGameFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.SavedGamesFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rws"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06005E33 RID: 24115 RVA: 0x002FE914 File Offset: 0x002FCD14
		public static IEnumerable<FileInfo> AllCustomScenarioFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ScenariosFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rsc"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06005E34 RID: 24116 RVA: 0x002FE990 File Offset: 0x002FCD90
		public static IEnumerable<FileInfo> AllExternalHistoryFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ExternalHistoryFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rwh"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x06005E35 RID: 24117 RVA: 0x002FEA0C File Offset: 0x002FCE0C
		private static string FolderUnderSaveData(string folderName)
		{
			string text = Path.Combine(GenFilePaths.SaveDataFolderPath, folderName);
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			return text;
		}

		// Token: 0x06005E36 RID: 24118 RVA: 0x002FEA48 File Offset: 0x002FCE48
		public static string FilePathForSavedGame(string gameName)
		{
			return Path.Combine(GenFilePaths.SavedGamesFolderPath, gameName + ".rws");
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x002FEA74 File Offset: 0x002FCE74
		public static string AbsPathForScenario(string scenarioName)
		{
			return Path.Combine(GenFilePaths.ScenariosFolderPath, scenarioName + ".rsc");
		}

		// Token: 0x06005E38 RID: 24120 RVA: 0x002FEAA0 File Offset: 0x002FCEA0
		public static string ContentPath<T>()
		{
			string result;
			if (typeof(T) == typeof(AudioClip))
			{
				result = "Sounds/";
			}
			else if (typeof(T) == typeof(Texture2D))
			{
				result = "Textures/";
			}
			else
			{
				if (typeof(T) != typeof(string))
				{
					throw new ArgumentException();
				}
				result = "Strings/";
			}
			return result;
		}

		// Token: 0x06005E39 RID: 24121 RVA: 0x002FEB24 File Offset: 0x002FCF24
		public static string FolderPathRelativeToDefsFolder(string fullFolderPath, ModContentPack mod)
		{
			fullFolderPath = Path.GetFullPath(fullFolderPath).Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
			string text = Path.GetFullPath(Path.Combine(mod.RootDir, "Defs/")).Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
			if (!fullFolderPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				fullFolderPath += Path.DirectorySeparatorChar;
			}
			if (!text.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				text += Path.DirectorySeparatorChar;
			}
			string result;
			if (!fullFolderPath.StartsWith(text))
			{
				Log.Error(string.Concat(new string[]
				{
					"Can't get relative path. Path \"",
					fullFolderPath,
					"\" does not start with \"",
					text,
					"\"."
				}), false);
				result = null;
			}
			else if (fullFolderPath == text)
			{
				result = "";
			}
			else
			{
				string text2 = fullFolderPath.Substring(text.Length);
				while (text2.StartsWith("/") || text2.StartsWith("\\"))
				{
					if (text2.Length == 1)
					{
						return "";
					}
					text2 = text2.Substring(1);
				}
				result = text2;
			}
			return result;
		}

		// Token: 0x06005E3A RID: 24122 RVA: 0x002FECA0 File Offset: 0x002FD0A0
		public static string SafeURIForUnityWWWFromPath(string rawPath)
		{
			string text = rawPath;
			for (int i = 0; i < GenFilePaths.FilePathRaw.Length; i++)
			{
				text = text.Replace(GenFilePaths.FilePathRaw[i], GenFilePaths.FilePathSafe[i]);
			}
			return "file:///" + text;
		}
	}
}
