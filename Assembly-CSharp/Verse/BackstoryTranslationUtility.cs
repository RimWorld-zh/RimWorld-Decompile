using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

namespace Verse
{
	public static class BackstoryTranslationUtility
	{
		[DebuggerHidden]
		private static IEnumerable<XElement> BackstoryTranslationElements(LoadedLanguage lang)
		{
			BackstoryTranslationUtility.<BackstoryTranslationElements>c__Iterator1EB <BackstoryTranslationElements>c__Iterator1EB = new BackstoryTranslationUtility.<BackstoryTranslationElements>c__Iterator1EB();
			<BackstoryTranslationElements>c__Iterator1EB.lang = lang;
			<BackstoryTranslationElements>c__Iterator1EB.<$>lang = lang;
			BackstoryTranslationUtility.<BackstoryTranslationElements>c__Iterator1EB expr_15 = <BackstoryTranslationElements>c__Iterator1EB;
			expr_15.$PC = -2;
			return expr_15;
		}

		public static void LoadAndInjectBackstoryData(LoadedLanguage lang)
		{
			foreach (XElement current in BackstoryTranslationUtility.BackstoryTranslationElements(lang))
			{
				string text = "[unknown]";
				try
				{
					text = current.Name.ToString();
					string value = current.Element("title").Value;
					string value2 = current.Element("titleShort").Value;
					string value3 = current.Element("desc").Value;
					Backstory backstory;
					if (!BackstoryDatabase.TryGetWithIdentifier(text, out backstory))
					{
						throw new Exception("Backstory not found matching identifier " + text);
					}
					if (value == backstory.Title && value2 == backstory.TitleShort && value3 == backstory.baseDesc)
					{
						Log.Error("Backstory translation exactly matches default data: " + text);
					}
					else
					{
						if (value != null)
						{
							backstory.SetTitle(value);
						}
						if (value2 != null)
						{
							backstory.SetTitleShort(value2);
						}
						if (value3 != null)
						{
							backstory.baseDesc = value3;
						}
					}
				}
				catch (Exception ex)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Couldn't load backstory ",
						text,
						": ",
						ex,
						"\nFull XML text:\n\n",
						current.ToString()
					}));
				}
			}
		}

		[DebuggerHidden]
		public static IEnumerable<string> MissingBackstoryTranslations(LoadedLanguage lang)
		{
			BackstoryTranslationUtility.<MissingBackstoryTranslations>c__Iterator1EC <MissingBackstoryTranslations>c__Iterator1EC = new BackstoryTranslationUtility.<MissingBackstoryTranslations>c__Iterator1EC();
			<MissingBackstoryTranslations>c__Iterator1EC.lang = lang;
			<MissingBackstoryTranslations>c__Iterator1EC.<$>lang = lang;
			BackstoryTranslationUtility.<MissingBackstoryTranslations>c__Iterator1EC expr_15 = <MissingBackstoryTranslations>c__Iterator1EC;
			expr_15.$PC = -2;
			return expr_15;
		}
	}
}
