using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F3E RID: 3902
	public static class GenFilePaths
	{
		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06005DF0 RID: 24048 RVA: 0x002FBC88 File Offset: 0x002FA088
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

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06005DF1 RID: 24049 RVA: 0x002FBDE0 File Offset: 0x002FA1E0
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

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06005DF2 RID: 24050 RVA: 0x002FBE44 File Offset: 0x002FA244
		private static DirectoryInfo ExecutableDir
		{
			get
			{
				return new DirectoryInfo(UnityData.dataPath).Parent;
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06005DF3 RID: 24051 RVA: 0x002FBE68 File Offset: 0x002FA268
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

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x06005DF4 RID: 24052 RVA: 0x002FBF28 File Offset: 0x002FA328
		public static string ConfigFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Config");
			}
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06005DF5 RID: 24053 RVA: 0x002FBF48 File Offset: 0x002FA348
		private static string SavedGamesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Saves");
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06005DF6 RID: 24054 RVA: 0x002FBF68 File Offset: 0x002FA368
		private static string ScenariosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Scenarios");
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06005DF7 RID: 24055 RVA: 0x002FBF88 File Offset: 0x002FA388
		private static string ExternalHistoryFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("External");
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06005DF8 RID: 24056 RVA: 0x002FBFA8 File Offset: 0x002FA3A8
		public static string ScreenshotFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Screenshots");
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06005DF9 RID: 24057 RVA: 0x002FBFC8 File Offset: 0x002FA3C8
		public static string DevOutputFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("DevOutput");
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x06005DFA RID: 24058 RVA: 0x002FBFE8 File Offset: 0x002FA3E8
		public static string ModsConfigFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "ModsConfig.xml");
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06005DFB RID: 24059 RVA: 0x002FC00C File Offset: 0x002FA40C
		public static string ConceptKnowledgeFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Knowledge.xml");
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06005DFC RID: 24060 RVA: 0x002FC030 File Offset: 0x002FA430
		public static string PrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Prefs.xml");
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06005DFD RID: 24061 RVA: 0x002FC054 File Offset: 0x002FA454
		public static string KeyPrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "KeyPrefs.xml");
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x06005DFE RID: 24062 RVA: 0x002FC078 File Offset: 0x002FA478
		public static string LastPlayedVersionFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "LastPlayedVersion.txt");
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06005DFF RID: 24063 RVA: 0x002FC09C File Offset: 0x002FA49C
		public static string DevModePermanentlyDisabledFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "DevModeDisabled");
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06005E00 RID: 24064 RVA: 0x002FC0C0 File Offset: 0x002FA4C0
		public static string BackstoryOutputFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.DevOutputFolderPath, "Fresh_Backstories.xml");
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06005E01 RID: 24065 RVA: 0x002FC0E4 File Offset: 0x002FA4E4
		public static string TempFolderPath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06005E02 RID: 24066 RVA: 0x002FC100 File Offset: 0x002FA500
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

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06005E03 RID: 24067 RVA: 0x002FC17C File Offset: 0x002FA57C
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

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06005E04 RID: 24068 RVA: 0x002FC1F8 File Offset: 0x002FA5F8
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

		// Token: 0x06005E05 RID: 24069 RVA: 0x002FC274 File Offset: 0x002FA674
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

		// Token: 0x06005E06 RID: 24070 RVA: 0x002FC2B0 File Offset: 0x002FA6B0
		public static string FilePathForSavedGame(string gameName)
		{
			return Path.Combine(GenFilePaths.SavedGamesFolderPath, gameName + ".rws");
		}

		// Token: 0x06005E07 RID: 24071 RVA: 0x002FC2DC File Offset: 0x002FA6DC
		public static string AbsPathForScenario(string scenarioName)
		{
			return Path.Combine(GenFilePaths.ScenariosFolderPath, scenarioName + ".rsc");
		}

		// Token: 0x06005E08 RID: 24072 RVA: 0x002FC308 File Offset: 0x002FA708
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

		// Token: 0x06005E09 RID: 24073 RVA: 0x002FC38C File Offset: 0x002FA78C
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

		// Token: 0x06005E0A RID: 24074 RVA: 0x002FC508 File Offset: 0x002FA908
		public static string SafeURIForUnityWWWFromPath(string rawPath)
		{
			string text = rawPath;
			for (int i = 0; i < GenFilePaths.FilePathRaw.Length; i++)
			{
				text = text.Replace(GenFilePaths.FilePathRaw[i], GenFilePaths.FilePathSafe[i]);
			}
			return "file:///" + text;
		}

		// Token: 0x04003DEE RID: 15854
		private static string saveDataPath = null;

		// Token: 0x04003DEF RID: 15855
		private static string coreModsFolderPath = null;

		// Token: 0x04003DF0 RID: 15856
		public const string SoundsFolder = "Sounds/";

		// Token: 0x04003DF1 RID: 15857
		public const string TexturesFolder = "Textures/";

		// Token: 0x04003DF2 RID: 15858
		public const string StringsFolder = "Strings/";

		// Token: 0x04003DF3 RID: 15859
		public const string DefsFolder = "Defs/";

		// Token: 0x04003DF4 RID: 15860
		public const string PatchesFolder = "Patches/";

		// Token: 0x04003DF5 RID: 15861
		public const string BackstoriesPath = "Backstories";

		// Token: 0x04003DF6 RID: 15862
		public const string SavedGameExtension = ".rws";

		// Token: 0x04003DF7 RID: 15863
		public const string ScenarioExtension = ".rsc";

		// Token: 0x04003DF8 RID: 15864
		public const string ExternalHistoryFileExtension = ".rwh";

		// Token: 0x04003DF9 RID: 15865
		private const string SaveDataFolderCommand = "savedatafolder";

		// Token: 0x04003DFA RID: 15866
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

		// Token: 0x04003DFB RID: 15867
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
	}
}
