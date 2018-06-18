using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BF5 RID: 3061
	public static class LanguageReportGenerator
	{
		// Token: 0x060042B8 RID: 17080 RVA: 0x00234200 File Offset: 0x00232600
		public static void SaveTranslationReport()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				Messages.Message("Please activate a non-English language to scan.", MessageTypeDefOf.RejectInput, false);
			}
			else
			{
				activeLanguage.LoadData();
				defaultLanguage.LoadData();
				if (LanguageReportGenerator.<>f__mg$cache0 == null)
				{
					LanguageReportGenerator.<>f__mg$cache0 = new Action(LanguageReportGenerator.DoSaveTranslationReport);
				}
				LongEventHandler.QueueLongEvent(LanguageReportGenerator.<>f__mg$cache0, "GeneratingTranslationReport", true, null);
			}
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x0023426C File Offset: 0x0023266C
		private static void DoSaveTranslationReport()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Translation report for " + activeLanguage);
			if (activeLanguage.defInjections.Any((DefInjectionPackage x) => x.usedOldRepSyntax))
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Consider using <Something.List.4.Field>translation</Something.List.4.Field> def-injection syntax instead of <rep>.");
			}
			try
			{
				LanguageReportGenerator.AppendGeneralLoadErrors(stringBuilder);
			}
			catch (Exception arg)
			{
				Log.Error("Error while generating translation report (general load errors): " + arg, false);
			}
			try
			{
				LanguageReportGenerator.AppendDefInjectionsLoadErros(stringBuilder);
			}
			catch (Exception arg2)
			{
				Log.Error("Error while generating translation report (def-injections load errors): " + arg2, false);
			}
			try
			{
				LanguageReportGenerator.AppendBackstoriesLoadErrors(stringBuilder);
			}
			catch (Exception arg3)
			{
				Log.Error("Error while generating translation report (backstories load errors): " + arg3, false);
			}
			try
			{
				LanguageReportGenerator.AppendMissingKeyedTranslations(stringBuilder);
			}
			catch (Exception arg4)
			{
				Log.Error("Error while generating translation report (missing keyed translations): " + arg4, false);
			}
			List<string> list = new List<string>();
			try
			{
				LanguageReportGenerator.AppendMissingDefInjections(stringBuilder, list);
			}
			catch (Exception arg5)
			{
				Log.Error("Error while generating translation report (missing def-injections): " + arg5, false);
			}
			try
			{
				LanguageReportGenerator.AppendMissingBackstories(stringBuilder);
			}
			catch (Exception arg6)
			{
				Log.Error("Error while generating translation report (missing backstories): " + arg6, false);
			}
			try
			{
				LanguageReportGenerator.AppendUnnecessaryDefInjections(stringBuilder, list);
			}
			catch (Exception arg7)
			{
				Log.Error("Error while generating translation report (unnecessary def-injections): " + arg7, false);
			}
			try
			{
				LanguageReportGenerator.AppendRenamedDefInjections(stringBuilder);
			}
			catch (Exception arg8)
			{
				Log.Error("Error while generating translation report (renamed def-injections): " + arg8, false);
			}
			try
			{
				LanguageReportGenerator.AppendArgumentCountMismatches(stringBuilder);
			}
			catch (Exception arg9)
			{
				Log.Error("Error while generating translation report (argument count mismatches): " + arg9, false);
			}
			try
			{
				LanguageReportGenerator.AppendUnnecessaryKeyedTranslations(stringBuilder);
			}
			catch (Exception arg10)
			{
				Log.Error("Error while generating translation report (unnecessary keyed translations): " + arg10, false);
			}
			try
			{
				LanguageReportGenerator.AppendKeyedTranslationsMatchingEnglish(stringBuilder);
			}
			catch (Exception arg11)
			{
				Log.Error("Error while generating translation report (keyed translations matching English): " + arg11, false);
			}
			try
			{
				LanguageReportGenerator.AppendBackstoriesMatchingEnglish(stringBuilder);
			}
			catch (Exception arg12)
			{
				Log.Error("Error while generating translation report (backstories matching English): " + arg12, false);
			}
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			if (text.NullOrEmpty())
			{
				text = GenFilePaths.SaveDataFolderPath;
			}
			text = Path.Combine(text, "TranslationReport.txt");
			File.WriteAllText(text, stringBuilder.ToString());
			Messages.Message("MessageTranslationReportSaved".Translate(new object[]
			{
				Path.GetFullPath(text)
			}), MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x002345B0 File Offset: 0x002329B0
		private static void AppendGeneralLoadErrors(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in activeLanguage.loadErrors)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== General load errors (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x00234650 File Offset: 0x00232A50
		private static void AppendDefInjectionsLoadErros(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string value in defInjectionPackage.loadErrors)
				{
					num++;
					stringBuilder.AppendLine(value);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations load errors (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x0023473C File Offset: 0x00232B3C
		private static void AppendBackstoriesLoadErrors(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in activeLanguage.backstoriesLoadErrors)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstories load errors (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x002347DC File Offset: 0x00232BDC
		private static void AppendMissingKeyedTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in defaultLanguage.keyedReplacements)
			{
				if (!activeLanguage.HaveTextForKey(keyValuePair.Key))
				{
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.Replace("\n", "\\n"),
						"' (",
						defaultLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Missing keyed translations (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x002348EC File Offset: 0x00232CEC
		private static void AppendMissingDefInjections(StringBuilder sb, List<string> outUnnecessaryDefInjections)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (string str in defInjectionPackage.MissingInjections(outUnnecessaryDefInjections))
				{
					num++;
					stringBuilder.AppendLine(defInjectionPackage.defType.Name + ": " + str);
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations missing (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x002349EC File Offset: 0x00232DEC
		private static void AppendMissingBackstories(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.MissingBackstoryTranslations(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstory translations missing (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x00234A8C File Offset: 0x00232E8C
		private static void AppendUnnecessaryDefInjections(StringBuilder sb, List<string> unnecessaryDefInjections)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in unnecessaryDefInjections)
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Unnecessary def-injected translations (marked as NoTranslate) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x00234B20 File Offset: 0x00232F20
		private static void AppendRenamedDefInjections(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DefInjectionPackage defInjectionPackage in activeLanguage.defInjections)
			{
				foreach (Pair<string, string> pair in defInjectionPackage.autoFixedBackCompatKeys)
				{
					string text = pair.First.Split(new char[]
					{
						'.'
					})[0];
					string text2 = pair.Second.Split(new char[]
					{
						'.'
					})[0];
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"Def has been renamed: ",
						text,
						" -> ",
						text2,
						", translation ",
						pair.First,
						" should be renamed as well."
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Def-injected translations using old, renamed defs (fixed automatically but can break in the next RimWorld version) (" + num + ") =========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x00234C7C File Offset: 0x0023307C
		private static void AppendArgumentCountMismatches(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string text in defaultLanguage.keyedReplacements.Keys.Intersect(activeLanguage.keyedReplacements.Keys))
			{
				int num2 = LanguageReportGenerator.CountParametersInString(defaultLanguage.keyedReplacements[text]);
				int num3 = LanguageReportGenerator.CountParametersInString(activeLanguage.keyedReplacements[text]);
				if (num2 != num3)
				{
					num++;
					stringBuilder.AppendLine(string.Format("{0} ({1})\n  - '{2}'\n  - '{3}'", new object[]
					{
						text,
						activeLanguage.GetKeySourceFileAndLine(text),
						defaultLanguage.keyedReplacements[text].Replace("\n", "\\n"),
						activeLanguage.keyedReplacements[text].Replace("\n", "\\n")
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Argument count mismatches (may or may not be incorrect) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x00234DC8 File Offset: 0x002331C8
		private static void AppendUnnecessaryKeyedTranslations(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in activeLanguage.keyedReplacements)
			{
				if (!defaultLanguage.HaveTextForKey(keyValuePair.Key))
				{
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.Replace("\n", "\\n"),
						"' (",
						activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Unnecessary keyed translations (will never be used) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x00234ED8 File Offset: 0x002332D8
		private static void AppendKeyedTranslationsMatchingEnglish(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in activeLanguage.keyedReplacements)
			{
				string b;
				if (defaultLanguage.TryGetTextFromKey(keyValuePair.Key, out b) && keyValuePair.Value == b)
				{
					num++;
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						keyValuePair.Key,
						" '",
						keyValuePair.Value.Replace("\n", "\\n"),
						"' (",
						activeLanguage.GetKeySourceFileAndLine(keyValuePair.Key),
						")"
					}));
				}
			}
			sb.AppendLine();
			sb.AppendLine("========== Keyed translations matching English (maybe ok) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x00234FFC File Offset: 0x002333FC
		private static void AppendBackstoriesMatchingEnglish(StringBuilder sb)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string value in BackstoryTranslationUtility.BackstoryTranslationsMatchingEnglish(activeLanguage))
			{
				num++;
				stringBuilder.AppendLine(value);
			}
			sb.AppendLine();
			sb.AppendLine("========== Backstory translations matching English (maybe ok) (" + num + ") ==========");
			sb.Append(stringBuilder);
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x0023509C File Offset: 0x0023349C
		public static int CountParametersInString(string input)
		{
			MatchCollection matchCollection = Regex.Matches(input, "(?<!\\{)\\{([0-9]+).*?\\}(?!})");
			int result;
			if (matchCollection.Count == 0)
			{
				result = 0;
			}
			else
			{
				result = matchCollection.Cast<Match>().Max((Match m) => int.Parse(m.Groups[1].Value)) + 1;
			}
			return result;
		}

		// Token: 0x04002DA2 RID: 11682
		private const string FileName = "TranslationReport.txt";

		// Token: 0x04002DA3 RID: 11683
		[CompilerGenerated]
		private static Action <>f__mg$cache0;
	}
}
