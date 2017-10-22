using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Verse
{
	public static class LanguageReportGenerator
	{
		public static void OutputTranslationReport()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			if (activeLanguage == defaultLanguage)
			{
				Messages.Message("Please activate a non-English language to scan.", MessageSound.RejectInput);
			}
			else
			{
				activeLanguage.LoadData();
				defaultLanguage.LoadData();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Translation report for " + activeLanguage);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("========== Argument count mismatches =========");
				foreach (string item in defaultLanguage.keyedReplacements.Keys.Intersect(activeLanguage.keyedReplacements.Keys))
				{
					int num = LanguageReportGenerator.CountParametersInString(defaultLanguage.keyedReplacements[item]);
					int num2 = LanguageReportGenerator.CountParametersInString(activeLanguage.keyedReplacements[item]);
					if (num != num2)
					{
						stringBuilder.AppendLine(string.Format("{0} - '{1}' compared to '{2}'", item, defaultLanguage.keyedReplacements[item], activeLanguage.keyedReplacements[item]));
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("========== Missing keyed translations =========");
				Dictionary<string, string>.Enumerator enumerator2 = defaultLanguage.keyedReplacements.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<string, string> current2 = enumerator2.Current;
						if (!activeLanguage.HaveTextForKey(current2.Key))
						{
							stringBuilder.AppendLine(current2.Key + " - '" + current2.Value + "'");
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("========== Unnecessary keyed translations (will never be used) =========");
				Dictionary<string, string>.Enumerator enumerator3 = activeLanguage.keyedReplacements.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						KeyValuePair<string, string> current3 = enumerator3.Current;
						if (!defaultLanguage.HaveTextForKey(current3.Key))
						{
							stringBuilder.AppendLine(current3.Key + " - '" + current3.Value + "'");
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator3).Dispose();
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("========== Def-injected translations missing =========");
				stringBuilder.AppendLine("Note: This does NOT return any kind of sub-fields. So if there's a list of strings, or a sub-member of the def with a string in it or something, they won't be reported here.");
				List<DefInjectionPackage>.Enumerator enumerator4 = activeLanguage.defInjections.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						DefInjectionPackage current4 = enumerator4.Current;
						foreach (string item2 in current4.MissingInjections())
						{
							stringBuilder.AppendLine(current4.defType.Name + ": " + item2);
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator4).Dispose();
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("========== Backstory translations missing =========");
				foreach (string item3 in BackstoryTranslationUtility.MissingBackstoryTranslations(activeLanguage))
				{
					stringBuilder.AppendLine(item3);
				}
				Log.Message(stringBuilder.ToString());
				Messages.Message("Translation report about " + activeLanguage.ToString() + " written to console. Hit ` to see it.", MessageSound.Standard);
			}
		}

		public static int CountParametersInString(string input)
		{
			MatchCollection matchCollection = Regex.Matches(input, "(?<!\\{)\\{([0-9]+).*?\\}(?!})");
			if (matchCollection.Count == 0)
			{
				return 0;
			}
			return matchCollection.Cast<Match>().Max((Func<Match, int>)((Match m) => int.Parse(m.Groups[1].Value))) + 1;
		}
	}
}
