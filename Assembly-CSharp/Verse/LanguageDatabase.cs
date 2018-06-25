using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000BF2 RID: 3058
	public static class LanguageDatabase
	{
		// Token: 0x04002DA3 RID: 11683
		private static List<LoadedLanguage> languages = new List<LoadedLanguage>();

		// Token: 0x04002DA4 RID: 11684
		public static LoadedLanguage activeLanguage;

		// Token: 0x04002DA5 RID: 11685
		public static LoadedLanguage defaultLanguage;

		// Token: 0x04002DA6 RID: 11686
		public static readonly string DefaultLangFolderName = "English";

		// Token: 0x04002DA7 RID: 11687
		private static readonly List<string> SupportedAutoSelectLanguages = new List<string>
		{
			"Arabic",
			"ChineseSimplified",
			"ChineseTraditional",
			"Czech",
			"Danish",
			"Dutch",
			"English",
			"Estonian",
			"Finnish",
			"French",
			"German",
			"Hungarian",
			"Italian",
			"Japanese",
			"Korean",
			"Norwegian",
			"Polish",
			"Portuguese",
			"PortugueseBrazilian",
			"Romanian",
			"Russian",
			"Slovak",
			"Spanish",
			"SpanishLatin",
			"Swedish",
			"Turkish",
			"Ukrainian"
		};

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x060042B1 RID: 17073 RVA: 0x00234B88 File Offset: 0x00232F88
		public static IEnumerable<LoadedLanguage> AllLoadedLanguages
		{
			get
			{
				return LanguageDatabase.languages;
			}
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x00234BA2 File Offset: 0x00232FA2
		public static void SelectLanguage(LoadedLanguage lang)
		{
			Prefs.LangFolderName = lang.folderName;
			LongEventHandler.QueueLongEvent(delegate()
			{
				PlayDataLoader.ClearAllPlayData();
				PlayDataLoader.LoadAllPlayData(false);
			}, "LoadingLongEvent", true, null);
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x00234BD9 File Offset: 0x00232FD9
		public static void Clear()
		{
			LanguageDatabase.languages.Clear();
			LanguageDatabase.activeLanguage = null;
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x00234BEC File Offset: 0x00232FEC
		public static void LoadAllMetadata()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				string path = Path.Combine(modContentPack.RootDir, "Languages");
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (directoryInfo.Exists)
				{
					foreach (DirectoryInfo langDir in directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly))
					{
						LanguageDatabase.LoadLanguageMetadataFrom(langDir);
					}
				}
			}
			LanguageDatabase.defaultLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == LanguageDatabase.DefaultLangFolderName);
			LanguageDatabase.activeLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == Prefs.LangFolderName);
			if (LanguageDatabase.activeLanguage == null)
			{
				Prefs.LangFolderName = LanguageDatabase.DefaultLangFolderName;
				LanguageDatabase.activeLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage la) => la.folderName == Prefs.LangFolderName);
			}
			if (LanguageDatabase.activeLanguage == null || LanguageDatabase.defaultLanguage == null)
			{
				Log.Error("No default language found!", false);
				LanguageDatabase.defaultLanguage = LanguageDatabase.languages[0];
				LanguageDatabase.activeLanguage = LanguageDatabase.languages[0];
			}
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x00234D7C File Offset: 0x0023317C
		private static LoadedLanguage LoadLanguageMetadataFrom(DirectoryInfo langDir)
		{
			LoadedLanguage loadedLanguage = LanguageDatabase.languages.FirstOrDefault((LoadedLanguage lib) => lib.folderName == langDir.Name);
			if (loadedLanguage == null)
			{
				loadedLanguage = new LoadedLanguage(langDir.ToString());
				LanguageDatabase.languages.Add(loadedLanguage);
			}
			if (loadedLanguage != null)
			{
				loadedLanguage.TryLoadMetadataFrom(langDir.ToString());
			}
			return loadedLanguage;
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x00234DF4 File Offset: 0x002331F4
		public static string SystemLanguageFolderName()
		{
			if (SteamManager.Initialized)
			{
				string text = SteamApps.GetCurrentGameLanguage().CapitalizeFirst();
				if (LanguageDatabase.SupportedAutoSelectLanguages.Contains(text))
				{
					return text;
				}
			}
			string text2 = Application.systemLanguage.ToString();
			string result;
			if (LanguageDatabase.SupportedAutoSelectLanguages.Contains(text2))
			{
				result = text2;
			}
			else
			{
				result = LanguageDatabase.DefaultLangFolderName;
			}
			return result;
		}
	}
}
