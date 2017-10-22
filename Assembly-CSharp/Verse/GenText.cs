using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	public static class GenText
	{
		private const int SaveNameMaxLength = 30;

		private const char DegreeSymbol = '°';

		private static StringBuilder tmpSb = new StringBuilder();

		public static string Possessive(this Pawn p)
		{
			return (p.gender != Gender.Male) ? "Proher".Translate() : "Prohis".Translate();
		}

		public static string PossessiveCap(this Pawn p)
		{
			return (p.gender != Gender.Male) ? "ProherCap".Translate() : "ProhisCap".Translate();
		}

		public static string ProObj(this Pawn p)
		{
			return (p.gender != Gender.Male) ? "ProherObj".Translate() : "ProhimObj".Translate();
		}

		public static string ProObjCap(this Pawn p)
		{
			return (p.gender != Gender.Male) ? "ProherObjCap".Translate() : "ProhimObjCap".Translate();
		}

		public static string ProSubj(this Pawn p)
		{
			return (p.gender != Gender.Male) ? "Proshe".Translate() : "Prohe".Translate();
		}

		public static string ProSubjCap(this Pawn p)
		{
			return (p.gender != Gender.Male) ? "ProsheCap".Translate() : "ProheCap".Translate();
		}

		public static string AdjustedFor(this string text, Pawn p)
		{
			return text.Replace("NAME", p.NameStringShort).Replace("HISCAP", p.PossessiveCap()).Replace("HIMCAP", p.ProObjCap()).Replace("HECAP", p.ProSubjCap()).Replace("HIS", p.Possessive()).Replace("HIM", p.ProObj()).Replace("HE", p.ProSubj());
		}

		public static string AdjustedForKeys(this string text)
		{
			string result;
			while (true)
			{
				int num = text.IndexOf("{Key:");
				if (num >= 0)
				{
					int num2 = num;
					while (text[num2] != '}')
					{
						num2++;
						if (num2 == text.Length - 1)
							goto IL_0035;
					}
					string defName = text.Substring(num + 5, num2 - (num + 5));
					KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(defName);
					if (namedSilentFail != null)
					{
						text = text.Substring(0, num) + namedSilentFail.MainKeyLabel + text.Substring(num2 + 1);
					}
					continue;
				}
				result = text;
				break;
				IL_0035:
				Log.Error("Cannot adjust for keys (mismatched braces): '" + text + "'");
				result = text;
				break;
			}
			return result;
		}

		public static string LabelIndefinite(this Pawn pawn)
		{
			string result;
			if (pawn.Name != null && !pawn.Name.Numerical)
			{
				result = pawn.LabelShort;
			}
			else
			{
				string str = Find.ActiveLanguageWorker.WithIndefiniteArticle(pawn.KindLabel);
				str = (result = Find.ActiveLanguageWorker.PostProcessed(str));
			}
			return result;
		}

		public static string LabelDefinite(this Pawn pawn)
		{
			string result;
			if (pawn.Name != null && !pawn.Name.Numerical)
			{
				result = pawn.LabelShort;
			}
			else
			{
				string str = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.KindLabel);
				str = (result = Find.ActiveLanguageWorker.PostProcessed(str));
			}
			return result;
		}

		public static string RandomSeedString()
		{
			return GrammarResolver.Resolve("seed", RulePackDefOf.SeedGenerator.Rules, null, (string)null).ToLower();
		}

		public static string WithoutVowels(string s)
		{
			string vowels = "aeiouy";
			return new string((from c in s
			where !vowels.Contains(c)
			select c).ToArray());
		}

		public static string WithoutVowelsIfLong(string s)
		{
			return (!s.NullOrEmpty() && s.Length > 5) ? (s.Substring(0, 2) + GenText.WithoutVowels(s.Substring(2))) : s;
		}

		public static string MarchingEllipsis(float offset = 0f)
		{
			string result;
			switch (Mathf.FloorToInt(Time.realtimeSinceStartup + offset) % 3)
			{
			case 0:
			{
				result = ".";
				break;
			}
			case 1:
			{
				result = "..";
				break;
			}
			case 2:
			{
				result = "...";
				break;
			}
			default:
			{
				throw new Exception();
			}
			}
			return result;
		}

		public static void SetTextSizeToFit(string text, Rect r)
		{
			Text.Font = GameFont.Small;
			float num = Text.CalcHeight(text, r.width);
			if (num > r.height)
			{
				Text.Font = GameFont.Tiny;
			}
		}

		public static string TrimEndNewlines(this string s)
		{
			return s.TrimEnd('\r', '\n');
		}

		public static string Indented(this string s)
		{
			return (!s.NullOrEmpty()) ? ("    " + s.Replace("\r", "").Replace("\n", "\n    ")) : s;
		}

		public static string ReplaceFirst(this string source, string key, string replacement)
		{
			int num = source.IndexOf(key);
			return (num >= 0) ? (source.Substring(0, num) + replacement + source.Substring(num + key.Length)) : source;
		}

		public static int StableStringHash(string str)
		{
			int result;
			if (str == null)
			{
				result = 0;
			}
			else
			{
				int num = 23;
				int length = str.Length;
				for (int num2 = 0; num2 < length; num2++)
				{
					num = num * 31 + str[num2];
				}
				result = num;
			}
			return result;
		}

		public static string StringFromEnumerable(IEnumerable source)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerator enumerator = source.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					stringBuilder.AppendLine("� " + current.ToString());
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return stringBuilder.ToString();
		}

		public static IEnumerable<string> LinesFromString(string text)
		{
			string[] lineSeparators = new string[2]
			{
				"\r\n",
				"\n"
			};
			string[] array = text.Split(lineSeparators, StringSplitOptions.RemoveEmptyEntries);
			int num = 0;
			string doneStr2;
			while (true)
			{
				if (num < array.Length)
				{
					string str = array[num];
					doneStr2 = str.Trim();
					if (!doneStr2.StartsWith("//"))
					{
						doneStr2 = doneStr2.Split(new string[1]
						{
							"//"
						}, StringSplitOptions.None)[0];
						if (doneStr2.Length != 0)
							break;
					}
					num++;
					continue;
				}
				yield break;
			}
			yield return doneStr2;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public static string GetInvalidFilenameCharacters()
		{
			return new string(Path.GetInvalidFileNameChars()) + "/\\{}<>:*|!@#$%^&*?";
		}

		public static bool IsValidFilename(string str)
		{
			bool result;
			if (str.Length > 30)
			{
				result = false;
			}
			else
			{
				Regex regex = new Regex("[" + Regex.Escape(GenText.GetInvalidFilenameCharacters()) + "]");
				result = !regex.IsMatch(str);
			}
			return result;
		}

		public static string SanitizeFilename(string str)
		{
			return string.Join("_", str.Split(GenText.GetInvalidFilenameCharacters().ToArray(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
		}

		public static bool NullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		public static string SplitCamelCase(string Str)
		{
			return Regex.Replace(Str, "(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", " ${a}");
		}

		public static string CapitalizedNoSpaces(string s)
		{
			string[] array = s.Split(' ');
			StringBuilder stringBuilder = new StringBuilder();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (text.Length > 0)
				{
					stringBuilder.Append(char.ToUpper(text[0]));
				}
				if (text.Length > 1)
				{
					stringBuilder.Append(text.Substring(1));
				}
			}
			return stringBuilder.ToString();
		}

		public static string RemoveNonAlphanumeric(string s)
		{
			GenText.tmpSb.Length = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsLetterOrDigit(s[i]))
				{
					GenText.tmpSb.Append(s[i]);
				}
			}
			return GenText.tmpSb.ToString();
		}

		public static bool EqualsIgnoreCase(this string A, string B)
		{
			return string.Compare(A, B, true) == 0;
		}

		public static string WithoutByteOrderMark(this string str)
		{
			return str.Trim().Trim('\ufeff');
		}

		public static bool NamesOverlap(string A, string B)
		{
			A = A.ToLower();
			B = B.ToLower();
			string[] array = A.Split(' ');
			string[] source = B.Split(' ');
			string[] array2 = array;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < array2.Length)
				{
					string text = array2[num];
					if (TitleCaseHelper.IsUppercaseTitleWord(text) && source.Contains(text))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static string CapitalizeFirst(this string str)
		{
			return (!str.NullOrEmpty()) ? ((!char.IsUpper(str[0])) ? ((str.Length != 1) ? (char.ToUpper(str[0]) + str.Substring(1)) : str.ToUpper()) : str) : str;
		}

		public static string ToNewsCase(string str)
		{
			string[] array = str.Split(' ');
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.Length >= 2)
				{
					if (i == 0)
					{
						array[i] = text[0].ToString().ToUpper() + text.Substring(1);
					}
					else
					{
						array[i] = text.ToLower();
					}
				}
			}
			return string.Join(" ", array);
		}

		public static string ToTitleCaseSmart(string str)
		{
			string[] array = str.Split(' ');
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (i == 0 || i == array.Length - 1 || TitleCaseHelper.IsUppercaseTitleWord(text))
				{
					string str2 = text[0].ToString().ToUpper();
					string str3 = text.Substring(1);
					array[i] = str2 + str3;
				}
			}
			return string.Join(" ", array);
		}

		public static string CapitalizeSentences(string input)
		{
			string result;
			if (input.NullOrEmpty())
			{
				result = input;
			}
			else if (input.Length == 1)
			{
				result = input.ToUpper();
			}
			else
			{
				input = Regex.Replace(input, "\\s+", " ");
				input = input.Trim();
				input = char.ToUpper(input[0]) + input.Substring(1);
				string[] array;
				string[] array2 = array = new string[3]
				{
					". ",
					"! ",
					"? "
				};
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					int length = text.Length;
					for (int num = input.IndexOf(text, 0); num > -1; num = input.IndexOf(text, num + 1))
					{
						input = input.Substring(0, num + length) + input[num + length].ToString().ToUpper() + input.Substring(num + length + 1);
					}
				}
				result = input;
			}
			return result;
		}

		public static string CapitalizeAsTitle(string str)
		{
			return Find.ActiveLanguageWorker.ToTitleCase(str);
		}

		public static string ToCommaList(IEnumerable<string> items, bool useAnd = true)
		{
			string text = (string)null;
			string text2 = (string)null;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			IList<string> list = items as IList<string>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					string text3 = list[i];
					if (!text3.NullOrEmpty())
					{
						if (text2 == null)
						{
							text2 = text3;
						}
						if (text != null)
						{
							stringBuilder.Append(text + ", ");
						}
						text = text3;
						num++;
					}
				}
			}
			else
			{
				foreach (string item in items)
				{
					if (!item.NullOrEmpty())
					{
						if (text2 == null)
						{
							text2 = item;
						}
						if (text != null)
						{
							stringBuilder.Append(text + ", ");
						}
						text = item;
						num++;
					}
				}
			}
			string result;
			switch (num)
			{
			case 0:
			{
				result = "NoneLower".Translate();
				break;
			}
			case 1:
			{
				result = text;
				break;
			}
			default:
			{
				if (useAnd)
				{
					if (num == 2)
					{
						result = text2 + " " + "AndLower".Translate() + " " + text;
					}
					else
					{
						stringBuilder.Append("AndLower".Translate() + " " + text);
						result = stringBuilder.ToString();
					}
				}
				else
				{
					stringBuilder.Append(text);
					result = stringBuilder.ToString();
				}
				break;
			}
			}
			return result;
		}

		public static string ToLineList(IEnumerable<string> entries, string prefix = "")
		{
			return GenText.ToTextList(entries, "\n" + prefix);
		}

		public static string ToSpaceList(IEnumerable<string> entries)
		{
			return GenText.ToTextList(entries, " ");
		}

		public static string ToTextList(IEnumerable<string> entries, string spacer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string item in entries)
			{
				if (!flag)
				{
					stringBuilder.Append(spacer);
				}
				stringBuilder.Append(item);
				flag = false;
			}
			return stringBuilder.ToString();
		}

		public static string ToCamelCase(string str)
		{
			str = GenText.ToTitleCaseSmart(str);
			return str.Replace(" ", (string)null);
		}

		public static string Truncate(this string str, float width, Dictionary<string, string> cache = null)
		{
			string text = default(string);
			string result;
			if (cache != null && cache.TryGetValue(str, out text))
			{
				result = text;
			}
			else
			{
				Vector2 vector = Text.CalcSize(str);
				if (vector.x <= width)
				{
					if (cache != null)
					{
						cache.Add(str, str);
					}
					result = str;
				}
				else
				{
					text = str;
					while (true)
					{
						text = text.Substring(0, text.Length - 1);
						if (text.Length <= 0)
							break;
						Vector2 vector2 = Text.CalcSize(text + "...");
						if (!(vector2.x > width))
							break;
					}
					text += "...";
					if (cache != null)
					{
						cache.Add(str, text);
					}
					result = text;
				}
			}
			return result;
		}

		public static string TrimmedToLength(this string str, int length)
		{
			return (str != null && str.Length > length) ? str.Substring(0, length) : str;
		}

		public static bool ContainsEmptyLines(string str)
		{
			return (byte)(str.NullOrEmpty() ? 1 : ((str[0] == '\n' || str[0] == '\r') ? 1 : ((str[str.Length - 1] == '\n' || str[str.Length - 1] == '\r') ? 1 : ((str.Contains("\n\n") || str.Contains("\r\n\r\n") || str.Contains("\r\r")) ? 1 : 0)))) != 0;
		}

		public static string ToStringByStyle(this float f, ToStringStyle style, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			if (style == ToStringStyle.Temperature && numberSense == ToStringNumberSense.Offset)
			{
				style = ToStringStyle.TemperatureOffset;
			}
			if (numberSense == ToStringNumberSense.Factor)
			{
				style = (ToStringStyle)((!(f >= 10.0)) ? 4 : 3);
			}
			string text;
			switch (style)
			{
			case ToStringStyle.Integer:
			{
				text = Mathf.RoundToInt(f).ToString();
				break;
			}
			case ToStringStyle.FloatOne:
			{
				text = f.ToString("F1");
				break;
			}
			case ToStringStyle.FloatTwo:
			{
				text = f.ToString("F2");
				break;
			}
			case ToStringStyle.FloatMaxTwo:
			{
				text = f.ToString("0.##");
				break;
			}
			case ToStringStyle.PercentZero:
			{
				text = f.ToStringPercent();
				break;
			}
			case ToStringStyle.PercentOne:
			{
				text = f.ToStringPercent("F1");
				break;
			}
			case ToStringStyle.PercentTwo:
			{
				text = f.ToStringPercent("F2");
				break;
			}
			case ToStringStyle.Temperature:
			{
				text = f.ToStringTemperature("F1");
				break;
			}
			case ToStringStyle.TemperatureOffset:
			{
				text = f.ToStringTemperatureOffset("F1");
				break;
			}
			case ToStringStyle.WorkAmount:
			{
				text = f.ToStringWorkAmount();
				break;
			}
			default:
			{
				Log.Error("Unknown ToStringStyle " + style);
				text = f.ToString();
				break;
			}
			}
			switch (numberSense)
			{
			case ToStringNumberSense.Offset:
			{
				if (f >= 0.0)
				{
					text = "+" + text;
				}
				break;
			}
			case ToStringNumberSense.Factor:
			{
				text = "x" + text;
				break;
			}
			}
			return text;
		}

		public static string ToStringDecimalIfSmall(this float f)
		{
			return (!(Mathf.Abs(f) < 1.0)) ? ((!(Mathf.Abs(f) < 10.0)) ? Mathf.RoundToInt(f).ToStringCached() : Math.Round((double)f, 1).ToString("0.#")) : Math.Round((double)f, 2).ToString("0.##");
		}

		public static string ToStringPercent(this float f)
		{
			return ((float)(f * 100.0)).ToStringDecimalIfSmall() + "%";
		}

		public static string ToStringPercent(this float f, string format)
		{
			return ((float)((f + 9.9999997473787516E-06) * 100.0)).ToString(format) + "%";
		}

		public static string ToStringMoney(this float f)
		{
			return "$" + f.ToString("F2");
		}

		public static string ToStringWithSign(this int i)
		{
			return i.ToString("+#;-#;0");
		}

		public static string ToStringKilobytes(this int bytes, string format = "F2")
		{
			return ((float)((float)bytes / 1024.0)).ToString(format) + "Kb";
		}

		public static string ToStringYesNo(this bool b)
		{
			return (!b) ? "No".Translate() : "Yes".Translate();
		}

		public static string ToStringLongitude(this float longitude)
		{
			bool flag = longitude < 0.0;
			if (flag)
			{
				longitude = (float)(0.0 - longitude);
			}
			return longitude.ToString("F2") + '°' + ((!flag) ? "E" : "W");
		}

		public static string ToStringLatitude(this float latitude)
		{
			bool flag = latitude < 0.0;
			if (flag)
			{
				latitude = (float)(0.0 - latitude);
			}
			return latitude.ToString("F2") + '°' + ((!flag) ? "N" : "S");
		}

		public static string ToStringMass(this float mass)
		{
			string result;
			if (mass == 0.0)
			{
				result = "0 g";
			}
			else
			{
				float num = Mathf.Abs(mass);
				if (num >= 100.0)
				{
					result = mass.ToString("F0") + " kg";
				}
				else if (num >= 10.0)
				{
					result = mass.ToString("0.#") + " kg";
				}
				else if (num >= 0.10000000149011612)
				{
					result = mass.ToString("0.##") + " kg";
				}
				else
				{
					float num2 = (float)(mass * 1000.0);
					result = ((!(num >= 0.0099999997764825821)) ? ((!(num >= 0.0010000000474974513)) ? (num2.ToString("0.##") + " g") : (num2.ToString("0.#") + " g")) : (num2.ToString("F0") + " g"));
				}
			}
			return result;
		}

		public static string ToStringMassOffset(this float mass)
		{
			string text = mass.ToStringMass();
			return (!(mass > 0.0)) ? text : ("+" + text);
		}

		public static string ToStringSign(this float val)
		{
			return (!(val >= 0.0)) ? "" : "+";
		}

		public static string ToStringTemperature(this float celsiusTemp, string format = "F1")
		{
			celsiusTemp = GenTemperature.CelsiusTo(celsiusTemp, Prefs.TemperatureMode);
			return celsiusTemp.ToStringTemperatureRaw(format);
		}

		public static string ToStringTemperatureOffset(this float celsiusTemp, string format = "F1")
		{
			celsiusTemp = GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode);
			return celsiusTemp.ToStringTemperatureRaw(format);
		}

		public static string ToStringTemperatureRaw(this float temp, string format = "F1")
		{
			string result;
			switch (Prefs.TemperatureMode)
			{
			case TemperatureDisplayMode.Celsius:
			{
				result = temp.ToString(format) + "C";
				break;
			}
			case TemperatureDisplayMode.Fahrenheit:
			{
				result = temp.ToString(format) + "F";
				break;
			}
			case TemperatureDisplayMode.Kelvin:
			{
				result = temp.ToString(format) + "K";
				break;
			}
			default:
			{
				throw new InvalidOperationException();
			}
			}
			return result;
		}

		public static string ToStringTwoDigits(this Vector2 v)
		{
			return "(" + v.x.ToString("F2") + ", " + v.y.ToString("F2") + ")";
		}

		public static string ToStringWorkAmount(this float workAmount)
		{
			return Mathf.CeilToInt((float)(workAmount / 60.0)).ToString();
		}

		public static string ToStringBytes(this int b, string format = "F2")
		{
			return ((float)((float)b / 8.0 / 1024.0)).ToString(format) + "kb";
		}

		public static string ToStringBytes(this uint b, string format = "F2")
		{
			return ((float)((float)(double)b / 8.0 / 1024.0)).ToString(format) + "kb";
		}

		public static string ToStringBytes(this long b, string format = "F2")
		{
			return ((float)((float)b / 8.0 / 1024.0)).ToString(format) + "kb";
		}

		public static string ToStringBytes(this ulong b, string format = "F2")
		{
			return ((float)((float)(double)b / 8.0 / 1024.0)).ToString(format) + "kb";
		}

		public static string ToStringReadable(this KeyCode k)
		{
			string result;
			switch (k)
			{
			case KeyCode.Keypad0:
			{
				result = "Kp0";
				break;
			}
			case KeyCode.Keypad1:
			{
				result = "Kp1";
				break;
			}
			case KeyCode.Keypad2:
			{
				result = "Kp2";
				break;
			}
			case KeyCode.Keypad3:
			{
				result = "Kp3";
				break;
			}
			case KeyCode.Keypad4:
			{
				result = "Kp4";
				break;
			}
			case KeyCode.Keypad5:
			{
				result = "Kp5";
				break;
			}
			case KeyCode.Keypad6:
			{
				result = "Kp6";
				break;
			}
			case KeyCode.Keypad7:
			{
				result = "Kp7";
				break;
			}
			case KeyCode.Keypad8:
			{
				result = "Kp8";
				break;
			}
			case KeyCode.Keypad9:
			{
				result = "Kp9";
				break;
			}
			case KeyCode.KeypadDivide:
			{
				result = "Kp/";
				break;
			}
			case KeyCode.KeypadEnter:
			{
				result = "KpEnt";
				break;
			}
			case KeyCode.KeypadEquals:
			{
				result = "Kp=";
				break;
			}
			case KeyCode.KeypadMinus:
			{
				result = "Kp-";
				break;
			}
			case KeyCode.KeypadMultiply:
			{
				result = "Kp*";
				break;
			}
			case KeyCode.KeypadPeriod:
			{
				result = "Kp.";
				break;
			}
			case KeyCode.KeypadPlus:
			{
				result = "Kp+";
				break;
			}
			case KeyCode.Alpha0:
			{
				result = "0";
				break;
			}
			case KeyCode.Alpha1:
			{
				result = "1";
				break;
			}
			case KeyCode.Alpha2:
			{
				result = "2";
				break;
			}
			case KeyCode.Alpha3:
			{
				result = "3";
				break;
			}
			case KeyCode.Alpha4:
			{
				result = "4";
				break;
			}
			case KeyCode.Alpha5:
			{
				result = "5";
				break;
			}
			case KeyCode.Alpha6:
			{
				result = "6";
				break;
			}
			case KeyCode.Alpha7:
			{
				result = "7";
				break;
			}
			case KeyCode.Alpha8:
			{
				result = "8";
				break;
			}
			case KeyCode.Alpha9:
			{
				result = "9";
				break;
			}
			case KeyCode.Clear:
			{
				result = "Clr";
				break;
			}
			case KeyCode.Backspace:
			{
				result = "Bksp";
				break;
			}
			case KeyCode.Return:
			{
				result = "Ent";
				break;
			}
			case KeyCode.Escape:
			{
				result = "Esc";
				break;
			}
			case KeyCode.DoubleQuote:
			{
				result = "\"";
				break;
			}
			case KeyCode.Exclaim:
			{
				result = "!";
				break;
			}
			case KeyCode.Hash:
			{
				result = "#";
				break;
			}
			case KeyCode.Dollar:
			{
				result = "$";
				break;
			}
			case KeyCode.Ampersand:
			{
				result = "&";
				break;
			}
			case KeyCode.Quote:
			{
				result = "'";
				break;
			}
			case KeyCode.LeftParen:
			{
				result = "(";
				break;
			}
			case KeyCode.RightParen:
			{
				result = ")";
				break;
			}
			case KeyCode.Asterisk:
			{
				result = "*";
				break;
			}
			case KeyCode.Plus:
			{
				result = "+";
				break;
			}
			case KeyCode.Minus:
			{
				result = "-";
				break;
			}
			case KeyCode.Comma:
			{
				result = ",";
				break;
			}
			case KeyCode.Period:
			{
				result = ".";
				break;
			}
			case KeyCode.Slash:
			{
				result = "/";
				break;
			}
			case KeyCode.Colon:
			{
				result = ":";
				break;
			}
			case KeyCode.Semicolon:
			{
				result = ";";
				break;
			}
			case KeyCode.Less:
			{
				result = "<";
				break;
			}
			case KeyCode.Greater:
			{
				result = ">";
				break;
			}
			case KeyCode.Question:
			{
				result = "?";
				break;
			}
			case KeyCode.At:
			{
				result = "@";
				break;
			}
			case KeyCode.LeftBracket:
			{
				result = "[";
				break;
			}
			case KeyCode.RightBracket:
			{
				result = "]";
				break;
			}
			case KeyCode.Backslash:
			{
				result = "\\";
				break;
			}
			case KeyCode.Caret:
			{
				result = "^";
				break;
			}
			case KeyCode.Underscore:
			{
				result = "_";
				break;
			}
			case KeyCode.BackQuote:
			{
				result = "`";
				break;
			}
			case KeyCode.Delete:
			{
				result = "Del";
				break;
			}
			case KeyCode.UpArrow:
			{
				result = "Up";
				break;
			}
			case KeyCode.DownArrow:
			{
				result = "Down";
				break;
			}
			case KeyCode.LeftArrow:
			{
				result = "Left";
				break;
			}
			case KeyCode.RightArrow:
			{
				result = "Right";
				break;
			}
			case KeyCode.Insert:
			{
				result = "Ins";
				break;
			}
			case KeyCode.Home:
			{
				result = "Home";
				break;
			}
			case KeyCode.End:
			{
				result = "End";
				break;
			}
			case KeyCode.PageDown:
			{
				result = "PgDn";
				break;
			}
			case KeyCode.PageUp:
			{
				result = "PgUp";
				break;
			}
			case KeyCode.Numlock:
			{
				result = "NumL";
				break;
			}
			case KeyCode.CapsLock:
			{
				result = "CapL";
				break;
			}
			case KeyCode.ScrollLock:
			{
				result = "ScrL";
				break;
			}
			case KeyCode.RightShift:
			{
				result = "RShf";
				break;
			}
			case KeyCode.LeftShift:
			{
				result = "LShf";
				break;
			}
			case KeyCode.RightControl:
			{
				result = "RCtrl";
				break;
			}
			case KeyCode.LeftControl:
			{
				result = "LCtrl";
				break;
			}
			case KeyCode.RightAlt:
			{
				result = "RAlt";
				break;
			}
			case KeyCode.LeftAlt:
			{
				result = "LAlt";
				break;
			}
			case KeyCode.RightCommand:
			{
				result = "Appl";
				break;
			}
			case KeyCode.LeftCommand:
			{
				result = "Cmd";
				break;
			}
			case KeyCode.LeftWindows:
			{
				result = "Win";
				break;
			}
			case KeyCode.RightWindows:
			{
				result = "Win";
				break;
			}
			case KeyCode.AltGr:
			{
				result = "AltGr";
				break;
			}
			case KeyCode.Help:
			{
				result = "Help";
				break;
			}
			case KeyCode.Print:
			{
				result = "Prnt";
				break;
			}
			case KeyCode.SysReq:
			{
				result = "SysReq";
				break;
			}
			case KeyCode.Break:
			{
				result = "Brk";
				break;
			}
			case KeyCode.Menu:
			{
				result = "Menu";
				break;
			}
			default:
			{
				result = k.ToString();
				break;
			}
			}
			return result;
		}

		public static void AppendWithComma(this StringBuilder sb, string text)
		{
			sb.AppendWithSeparator(text, ", ");
		}

		public static void AppendWithSeparator(this StringBuilder sb, string text, string separator)
		{
			if (!text.NullOrEmpty())
			{
				if (sb.Length > 0)
				{
					sb.Append(separator);
				}
				sb.Append(text);
			}
		}
	}
}
