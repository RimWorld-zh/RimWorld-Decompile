using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	public static class LanguageDatabase
	{
		private static List<LoadedLanguage> languages = new List<LoadedLanguage>();

		public static LoadedLanguage activeLanguage;

		public static LoadedLanguage defaultLanguage;

		public static readonly string DefaultLangFolderName = "English";

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

		[CompilerGenerated]
		private static Action <>f__am$cache0;

		[CompilerGenerated]
		private static Func<LoadedLanguage, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<LoadedLanguage, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<LoadedLanguage, bool> <>f__am$cache3;

		public static IEnumerable<LoadedLanguage> AllLoadedLanguages
		{
			get
			{
				return LanguageDatabase.languages;
			}
		}

		public static void SelectLanguage(LoadedLanguage lang)
		{
			Prefs.LangFolderName = lang.folderName;
			LongEventHandler.QueueLongEvent(delegate()
			{
				PlayDataLoader.ClearAllPlayData();
				PlayDataLoader.LoadAllPlayData(false);
			}, "LoadingLongEvent", true, null);
		}

		public static void Clear()
		{
			LanguageDatabase.languages.Clear();
			LanguageDatabase.activeLanguage = null;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static LanguageDatabase()
		{
		}

		[CompilerGenerated]
		private static void <SelectLanguage>m__0()
		{
			PlayDataLoader.ClearAllPlayData();
			PlayDataLoader.LoadAllPlayData(false);
		}

		[CompilerGenerated]
		private static bool <LoadAllMetadata>m__1(LoadedLanguage la)
		{
			return la.folderName == LanguageDatabase.DefaultLangFolderName;
		}

		[CompilerGenerated]
		private static bool <LoadAllMetadata>m__2(LoadedLanguage la)
		{
			return la.folderName == Prefs.LangFolderName;
		}

		[CompilerGenerated]
		private static bool <LoadAllMetadata>m__3(LoadedLanguage la)
		{
			return la.folderName == Prefs.LangFolderName;
		}

		[CompilerGenerated]
		private sealed class <LoadLanguageMetadataFrom>c__AnonStorey0
		{
			internal DirectoryInfo langDir;

			public <LoadLanguageMetadataFrom>c__AnonStorey0()
			{
			}

			internal bool <>m__0(LoadedLanguage lib)
			{
				return lib.folderName == this.langDir.Name;
			}
		}
	}
}
