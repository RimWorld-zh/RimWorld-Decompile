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
				Log.Error("No active language! Cannot translate from key " + key + ".");
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
			string text = default(string);
			if (key.NullOrEmpty())
			{
				result = key;
			}
			else if (LanguageDatabase.activeLanguage == null)
			{
				Log.Error("No active language! Cannot translate from key " + key + ".");
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
			if (key == null || key == "")
			{
				result = key;
			}
			else if (LanguageDatabase.activeLanguage == null)
			{
				Log.Error("No active language! Cannot translate from key " + key + ".");
				result = key;
			}
			else
			{
				string text = default(string);
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
					Log.Error("Exception translating '" + text + "': " + ex.ToString());
				}
				result = text2;
			}
			return result;
		}

		public static bool TryGetTranslatedStringsForFile(string fileName, out List<string> stringList)
		{
			bool result;
			if (!LanguageDatabase.activeLanguage.TryGetStringsFromFile(fileName, out stringList) && !LanguageDatabase.defaultLanguage.TryGetStringsFromFile(fileName, out stringList))
			{
				Log.Error("No string files for " + fileName + ".");
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
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
						text = "" + c;
						break;
					}
					}
					stringBuilder.Append(text);
				}
				result = stringBuilder.ToString();
			}
			return result;
		}
	}
}
