using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	public static class Translator
	{
		public static bool CanTranslate(this string key)
		{
			return LanguageDatabase.activeLanguage.HaveTextForKey(key);
		}

		public static bool TryTranslate(this string key, out string result)
		{
			bool result2;
			if (key.NullOrEmpty())
			{
				result = key;
				result2 = false;
			}
			else if (LanguageDatabase.activeLanguage == null)
			{
				Log.Error("No active language! Cannot translate from key " + key + ".", false);
				result = key;
				result2 = true;
			}
			else if (LanguageDatabase.activeLanguage.TryGetTextFromKey(key, out result))
			{
				result2 = true;
			}
			else
			{
				result = key;
				result2 = false;
			}
			return result2;
		}

		public static string Translate(this string key)
		{
			string result;
			string text;
			if (key.NullOrEmpty())
			{
				result = key;
			}
			else if (LanguageDatabase.activeLanguage == null)
			{
				Log.Error("No active language! Cannot translate from key " + key + ".", false);
				result = key;
			}
			else if (LanguageDatabase.activeLanguage.TryGetTextFromKey(key, out text))
			{
				result = text;
			}
			else
			{
				LanguageDatabase.defaultLanguage.TryGetTextFromKey(key, out text);
				if (Prefs.DevMode)
				{
					text = Translator.PseudoTranslated(text);
				}
				result = text;
			}
			return result;
		}

		public static string Translate(this string key, params object[] args)
		{
			string result;
			if (key.NullOrEmpty())
			{
				result = key;
			}
			else if (LanguageDatabase.activeLanguage == null)
			{
				Log.Error("No active language! Cannot translate from key " + key + ".", false);
				result = key;
			}
			else
			{
				string text;
				if (!LanguageDatabase.activeLanguage.TryGetTextFromKey(key, out text))
				{
					LanguageDatabase.defaultLanguage.TryGetTextFromKey(key, out text);
					if (Prefs.DevMode)
					{
						text = Translator.PseudoTranslated(text);
					}
				}
				string text2 = text;
				try
				{
					text2 = string.Format(text, args);
				}
				catch (Exception ex)
				{
					Log.Error("Exception translating '" + text + "': " + ex.ToString(), false);
				}
				result = text2;
			}
			return result;
		}

		public static bool TryGetTranslatedStringsForFile(string fileName, out List<string> stringList)
		{
			if (!LanguageDatabase.activeLanguage.TryGetStringsFromFile(fileName, out stringList))
			{
				if (!LanguageDatabase.defaultLanguage.TryGetStringsFromFile(fileName, out stringList))
				{
					Log.Error("No string files for " + fileName + ".", false);
					return false;
				}
			}
			return true;
		}

		private static string PseudoTranslated(string original)
		{
			string result;
			if (!Prefs.DevMode)
			{
				result = original;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (char c in original)
				{
					string value;
					switch (c)
					{
					case 'a':
						value = "à";
						break;
					case 'b':
						value = "þ";
						break;
					case 'c':
						value = "ç";
						break;
					case 'd':
						value = "ð";
						break;
					case 'e':
						value = "è";
						break;
					case 'f':
						value = "Ƒ";
						break;
					case 'g':
						value = "ğ";
						break;
					case 'h':
						value = "ĥ";
						break;
					case 'i':
						value = "ì";
						break;
					case 'j':
						value = "ĵ";
						break;
					case 'k':
						value = "к";
						break;
					case 'l':
						value = "ſ";
						break;
					case 'm':
						value = "ṁ";
						break;
					case 'n':
						value = "ƞ";
						break;
					case 'o':
						value = "ò";
						break;
					case 'p':
						value = "ṗ";
						break;
					case 'q':
						value = "q";
						break;
					case 'r':
						value = "ṟ";
						break;
					case 's':
						value = "ș";
						break;
					case 't':
						value = "ṭ";
						break;
					case 'u':
						value = "ù";
						break;
					case 'v':
						value = "ṽ";
						break;
					case 'w':
						value = "ẅ";
						break;
					case 'x':
						value = "ẋ";
						break;
					case 'y':
						value = "ý";
						break;
					case 'z':
						value = "ž";
						break;
					default:
						value = "" + c;
						break;
					}
					stringBuilder.Append(value);
				}
				result = stringBuilder.ToString();
			}
			return result;
		}
	}
}
