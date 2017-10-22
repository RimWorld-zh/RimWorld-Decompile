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
			if (key.NullOrEmpty())
			{
				result = key;
				return false;
			}
			if (LanguageDatabase.activeLanguage == null)
			{
				Log.Error("No active language! Cannot translate from key " + key + ".");
				result = key;
				return true;
			}
			if (LanguageDatabase.activeLanguage.TryGetTextFromKey(key, out result))
			{
				return true;
			}
			result = key;
			return false;
		}

		public static string Translate(this string key)
		{
			if (key.NullOrEmpty())
			{
				return key;
			}
			if (LanguageDatabase.activeLanguage == null)
			{
				Log.Error("No active language! Cannot translate from key " + key + ".");
				return key;
			}
			string text = default(string);
			if (LanguageDatabase.activeLanguage.TryGetTextFromKey(key, out text))
			{
				return text;
			}
			LanguageDatabase.defaultLanguage.TryGetTextFromKey(key, out text);
			if (Prefs.DevMode)
			{
				text = Translator.PseudoTranslated(text);
			}
			return text;
		}

		public static string Translate(this string key, params object[] args)
		{
			if (key != null && !(key == string.Empty))
			{
				if (LanguageDatabase.activeLanguage == null)
				{
					Log.Error("No active language! Cannot translate from key " + key + ".");
					return key;
				}
				string text = default(string);
				if (!LanguageDatabase.activeLanguage.TryGetTextFromKey(key, out text))
				{
					LanguageDatabase.defaultLanguage.TryGetTextFromKey(key, out text);
					if (Prefs.DevMode)
					{
						text = Translator.PseudoTranslated(text);
					}
				}
				string result = text;
				try
				{
					result = string.Format(text, args);
					return result;
				}
				catch (Exception ex)
				{
					Log.Error("Exception translating '" + text + "': " + ex.ToString());
					return result;
				}
			}
			return key;
		}

		public static bool TryGetTranslatedStringsForFile(string fileName, out List<string> stringList)
		{
			if (!LanguageDatabase.activeLanguage.TryGetStringsFromFile(fileName, out stringList) && !LanguageDatabase.defaultLanguage.TryGetStringsFromFile(fileName, out stringList))
			{
				Log.Error("No string files for " + fileName + ".");
				return false;
			}
			return true;
		}

		private static string PseudoTranslated(string original)
		{
			if (!Prefs.DevMode)
			{
				return original;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < original.Length; i++)
			{
				char c = original[i];
				string text = (string)null;
				switch (c)
				{
				case 'a':
				{
					text = "à";
					break;
				}
				case 'b':
				{
					text = "þ";
					break;
				}
				case 'c':
				{
					text = "ç";
					break;
				}
				case 'd':
				{
					text = "ð";
					break;
				}
				case 'e':
				{
					text = "è";
					break;
				}
				case 'f':
				{
					text = "Ƒ";
					break;
				}
				case 'g':
				{
					text = "ğ";
					break;
				}
				case 'h':
				{
					text = "ĥ";
					break;
				}
				case 'i':
				{
					text = "ì";
					break;
				}
				case 'j':
				{
					text = "ĵ";
					break;
				}
				case 'k':
				{
					text = "к";
					break;
				}
				case 'l':
				{
					text = "ſ";
					break;
				}
				case 'm':
				{
					text = "ṁ";
					break;
				}
				case 'n':
				{
					text = "ƞ";
					break;
				}
				case 'o':
				{
					text = "ò";
					break;
				}
				case 'p':
				{
					text = "ṗ";
					break;
				}
				case 'q':
				{
					text = "q";
					break;
				}
				case 'r':
				{
					text = "ṟ";
					break;
				}
				case 's':
				{
					text = "ș";
					break;
				}
				case 't':
				{
					text = "ṭ";
					break;
				}
				case 'u':
				{
					text = "ù";
					break;
				}
				case 'v':
				{
					text = "ṽ";
					break;
				}
				case 'w':
				{
					text = "ẅ";
					break;
				}
				case 'x':
				{
					text = "ẋ";
					break;
				}
				case 'y':
				{
					text = "ý";
					break;
				}
				case 'z':
				{
					text = "ž";
					break;
				}
				default:
				{
					text = string.Empty + c;
					break;
				}
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}
	}
}
