using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000BEF RID: 3055
	public static class LanguageDatabase
	{
		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x060042AE RID: 17070 RVA: 0x002347CC File Offset: 0x00232BCC
		public static IEnumerable<LoadedLanguage> AllLoadedLanguages
		{
			get
			{
				return LanguageDatabase.languages;
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x002347E6 File Offset: 0x00232BE6
		public static void SelectLanguage(LoadedLanguage lang)
		{
			Prefs.LangFolderName = lang.folderName;
			LongEventHandler.QueueLongEvent(delegate()
			{
				PlayDataLoader.ClearAllPlayData();
				PlayDataLoader.LoadAllPlayData(false);
			}, "LoadingLongEvent", true, null);
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x0023481D File Offset: 0x00232C1D
		public static void Clear()
		{
			LanguageDatabase.languages.Clear();
			LanguageDatabase.activeLanguage = null;
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x00234830 File Offset: 0x00232C30
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

		// Token: 0x060042B2 RID: 17074 RVA: 0x002349C0 File Offset: 0x00232DC0
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

		// Token: 0x060042B3 RID: 17075 RVA: 0x00234A38 File Offset: 0x00232E38
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

		// Token: 0x04002D9C RID: 11676
		private static List<LoadedLanguage> languages = new List<LoadedLanguage>();

		// Token: 0x04002D9D RID: 11677
		public static LoadedLanguage activeLanguage;

		// Token: 0x04002D9E RID: 11678
		public static LoadedLanguage defaultLanguage;

		// Token: 0x04002D9F RID: 11679
		public static readonly string DefaultLangFolderName = "English";

		// Token: 0x04002DA0 RID: 11680
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
	}
}
