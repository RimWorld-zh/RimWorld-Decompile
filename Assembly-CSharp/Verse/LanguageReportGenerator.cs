using RimWorld;
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
				Messages.Message("Please activate a non-English language to scan.", MessageTypeDefOf.RejectInput);
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
				foreach (KeyValuePair<string, string> keyedReplacement in defaultLanguage.keyedReplacements)
				{
					if (!activeLanguage.HaveTextForKey(keyedReplacement.Key))
					{
						stringBuilder.AppendLine(keyedReplacement.Key + " - '" + keyedReplacement.Value + "'");
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("========== Unnecessary keyed translations (will never be used) =========");
				foreach (KeyValuePair<string, string> keyedReplacement2 in activeLanguage.keyedReplacements)
				{
					if (!defaultLanguage.HaveTextForKey(keyedReplacement2.Key))
					{
						stringBuilder.AppendLine(keyedReplacement2.Key + " - '" + keyedReplacement2.Value + "'");
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("========== Def-injected translations missing =========");
				stringBuilder.AppendLine("Note: This does NOT return any kind of sub-fields. So if there's a list of strings, or a sub-member of the def with a string in it or something, they won't be reported here.");
				foreach (DefInjectionPackage defInjection in activeLanguage.defInjections)
				{
					foreach (string item2 in defInjection.MissingInjections())
					{
						stringBuilder.AppendLine(defInjection.defType.Name + ": " + item2);
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("========== Backstory translations missing =========");
				foreach (string item3 in BackstoryTranslationUtility.MissingBackstoryTranslations(activeLanguage))
				{
					stringBuilder.AppendLine(item3);
				}
				Log.Message(stringBuilder.ToString());
				Messages.Message("Translation report about " + activeLanguage.ToString() + " written to console. Hit ` to see it.", MessageTypeDefOf.NeutralEvent);
			}
		}

		public static int CountParametersInString(string input)
		{
			MatchCollection matchCollection = Regex.Matches(input, "(?<!\\{)\\{([0-9]+).*?\\}(?!})");
			if (matchCollection.Count == 0)
			{
				return 0;
			}
			return matchCollection.Cast<Match>().Max((Match m) => int.Parse(m.Groups[1].Value)) + 1;
		}
	}
}
