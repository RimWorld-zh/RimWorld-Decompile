using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	public static class GenText
	{
		private const int SaveNameMaxLength = 30;

		private const char DegreeSymbol = '°';

		private static StringBuilder tmpSb = new StringBuilder();

		private static StringBuilder tmpStringBuilder = new StringBuilder();

		[CompilerGenerated]
		private static Func<char, int, Pair<char, int>> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pair<char, int>, bool> <>f__am$cache1;

		public static string Possessive(this Pawn p)
		{
			string result;
			if (p.gender == Gender.Male)
			{
				result = "Prohis".Translate();
			}
			else
			{
				result = "Proher".Translate();
			}
			return result;
		}

		public static string PossessiveCap(this Pawn p)
		{
			string result;
			if (p.gender == Gender.Male)
			{
				result = "ProhisCap".Translate();
			}
			else
			{
				result = "ProherCap".Translate();
			}
			return result;
		}

		public static string ProObj(this Pawn p)
		{
			string result;
			if (p.gender == Gender.Male)
			{
				result = "ProhimObj".Translate();
			}
			else
			{
				result = "ProherObj".Translate();
			}
			return result;
		}

		public static string ProObjCap(this Pawn p)
		{
			string result;
			if (p.gender == Gender.Male)
			{
				result = "ProhimObjCap".Translate();
			}
			else
			{
				result = "ProherObjCap".Translate();
			}
			return result;
		}

		public static string ProSubj(this Pawn p)
		{
			string result;
			if (p.gender == Gender.Male)
			{
				result = "Prohe".Translate();
			}
			else
			{
				result = "Proshe".Translate();
			}
			return result;
		}

		public static string ProSubjCap(this Pawn p)
		{
			string result;
			if (p.gender == Gender.Male)
			{
				result = "ProheCap".Translate();
			}
			else
			{
				result = "ProsheCap".Translate();
			}
			return result;
		}

		public static string AdjustedFor(this string text, Pawn p, string pawnSymbol = "PAWN")
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.DynamicWrapper);
			request.Rules.Add(new Rule_String("RULE", text));
			request.Rules.AddRange(GrammarUtility.RulesForPawn(pawnSymbol, p, null));
			return GrammarResolver.Resolve("r_root", request, null, false);
		}

		public static string AdjustedForKeys(this string text, List<string> outErrors = null, bool resolveKeys = true)
		{
			if (outErrors != null)
			{
				outErrors.Clear();
			}
			string result;
			if (text.NullOrEmpty())
			{
				result = text;
			}
			else
			{
				int num = 0;
				for (;;)
				{
					num++;
					if (num > 500000)
					{
						break;
					}
					int num2 = text.IndexOf("{Key:");
					if (num2 < 0)
					{
						goto Block_5;
					}
					int num3 = num2;
					while (text[num3] != '}')
					{
						num3++;
						if (num3 >= text.Length)
						{
							goto Block_6;
						}
					}
					string text2 = text.Substring(num2 + 5, num3 - (num2 + 5));
					KeyBindingDef namedSilentFail = DefDatabase<KeyBindingDef>.GetNamedSilentFail(text2);
					string text3 = text.Substring(0, num2);
					if (namedSilentFail != null)
					{
						if (resolveKeys)
						{
							text3 += namedSilentFail.MainKeyLabel;
						}
						else
						{
							text3 += "placeholder";
						}
					}
					else
					{
						text3 += "error";
						if (outErrors != null)
						{
							string text4 = "Could not find key '" + text2 + "'";
							string text5 = BackCompatibility.BackCompatibleDefName(typeof(KeyBindingDef), text2, false);
							if (text5 != text2)
							{
								text4 = text4 + " (hint: it was renamed to '" + text5 + "')";
							}
							outErrors.Add(text4);
						}
					}
					text3 += text.Substring(num3 + 1);
					text = text3;
				}
				Log.Error("Too many iterations.", false);
				if (outErrors != null)
				{
					outErrors.Add("The parsed string caused an infinite loop");
				}
				Block_5:
				goto IL_18C;
				Block_6:
				if (outErrors != null)
				{
					outErrors.Add("Mismatched braces");
				}
				return text;
				IL_18C:
				result = text;
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
				result = pawn.KindLabelIndefinite();
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
				result = pawn.KindLabelDefinite();
			}
			return result;
		}

		public static string KindLabelIndefinite(this Pawn pawn)
		{
			return Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(pawn.KindLabel);
		}

		public static string KindLabelDefinite(this Pawn pawn)
		{
			return Find.ActiveLanguageWorker.WithDefiniteArticlePostProcessed(pawn.KindLabel);
		}

		public static string RandomSeedString()
		{
			return GrammarResolver.Resolve("r_seed", new GrammarRequest
			{
				Includes = 
				{
					RulePackDefOf.SeedGenerator
				}
			}, null, false).ToLower();
		}

		public static string Shorten(this string s)
		{
			string result;
			if (s.NullOrEmpty() || s.Length <= 4)
			{
				result = s;
			}
			else
			{
				s = s.Trim();
				string[] array = s.Split(new char[]
				{
					' '
				});
				string text = "";
				for (int i = 0; i < array.Length; i++)
				{
					if (i > 0)
					{
						text += " ";
					}
					if (array[i].Length > 2)
					{
						text = text + array[i].Substring(0, 1) + array[i].Substring(1, array[i].Length - 2).WithoutVowels() + array[i].Substring(array[i].Length - 1, 1);
					}
				}
				result = text;
			}
			return result;
		}

		private static string WithoutVowels(this string s)
		{
			string vowels = "aeiouy";
			return new string((from c in s
			where !vowels.Contains(c)
			select c).ToArray<char>());
		}

		public static string MarchingEllipsis(float offset = 0f)
		{
			int num = Mathf.FloorToInt(Time.realtimeSinceStartup + offset) % 3;
			string result;
			if (num != 0)
			{
				if (num != 1)
				{
					if (num != 2)
					{
						throw new Exception();
					}
					result = "...";
				}
				else
				{
					result = "..";
				}
			}
			else
			{
				result = ".";
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
			return s.TrimEnd(new char[]
			{
				'\r',
				'\n'
			});
		}

		public static string Indented(this string s, string indentation = "    ")
		{
			string result;
			if (s.NullOrEmpty())
			{
				result = s;
			}
			else
			{
				result = indentation + s.Replace("\r", "").Replace("\n", "\n" + indentation);
			}
			return result;
		}

		public static string ReplaceFirst(this string source, string key, string replacement)
		{
			int num = source.IndexOf(key);
			string result;
			if (num < 0)
			{
				result = source;
			}
			else
			{
				result = source.Substring(0, num) + replacement + source.Substring(num + key.Length);
			}
			return result;
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
				for (int i = 0; i < length; i++)
				{
					num = num * 31 + (int)str[i];
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
					object obj = enumerator.Current;
					stringBuilder.AppendLine("• " + obj.ToString());
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
			string[] lineSeparators = new string[]
			{
				"\r\n",
				"\n"
			};
			foreach (string str in text.Split(lineSeparators, StringSplitOptions.RemoveEmptyEntries))
			{
				string doneStr = str.Trim();
				if (!doneStr.StartsWith("//"))
				{
					doneStr = doneStr.Split(new string[]
					{
						"//"
					}, StringSplitOptions.None)[0];
					if (doneStr.Length != 0)
					{
						yield return doneStr;
					}
				}
			}
			yield break;
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
			return string.Join("_", str.Split(GenText.GetInvalidFilenameCharacters().ToArray<char>(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd(new char[]
			{
				'.'
			});
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
			string[] array = s.Split(new char[]
			{
				' '
			});
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in array)
			{
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
			return str.Trim().Trim(new char[]
			{
				'﻿'
			});
		}

		public static bool NamesOverlap(string A, string B)
		{
			A = A.ToLower();
			B = B.ToLower();
			string[] array = A.Split(new char[]
			{
				' '
			});
			string[] source = B.Split(new char[]
			{
				' '
			});
			foreach (string text in array)
			{
				if (TitleCaseHelper.IsUppercaseTitleWord(text))
				{
					if (source.Contains(text))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static string CapitalizeFirst(this string str)
		{
			string result;
			if (str.NullOrEmpty())
			{
				result = str;
			}
			else if (char.IsUpper(str[0]))
			{
				result = str;
			}
			else if (str.Length == 1)
			{
				result = str.ToUpper();
			}
			else
			{
				result = char.ToUpper(str[0]) + str.Substring(1);
			}
			return result;
		}

		public static string UncapitalizeFirst(this string str)
		{
			string result;
			if (str.NullOrEmpty())
			{
				result = str;
			}
			else if (char.IsLower(str[0]))
			{
				result = str;
			}
			else if (str.Length == 1)
			{
				result = str.ToLower();
			}
			else
			{
				result = char.ToLower(str[0]) + str.Substring(1);
			}
			return result;
		}

		public static string ToNewsCase(string str)
		{
			string[] array = str.Split(new char[]
			{
				' '
			});
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
			string[] array = str.MergeMultipleSpaces(false).Trim().Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (i == 0 || i == array.Length - 1 || TitleCaseHelper.IsUppercaseTitleWord(text))
				{
					if (!text.NullOrEmpty())
					{
						string str2 = text[0].ToString().ToUpper();
						string str3 = text.Substring(1);
						array[i] = str2 + str3;
					}
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
				bool flag = true;
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < input.Length; i++)
				{
					if (flag && char.IsLetterOrDigit(input[i]))
					{
						stringBuilder.Append(char.ToUpper(input[i]));
						flag = false;
					}
					else
					{
						stringBuilder.Append(input[i]);
					}
					if (input[i] == '\r' || input[i] == '\n' || input[i] == '.' || input[i] == '!' || input[i] == '?')
					{
						flag = true;
					}
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		public static string CapitalizeAsTitle(string str)
		{
			return Find.ActiveLanguageWorker.ToTitleCase(str);
		}

		public static string ToCommaList(this IEnumerable<string> items, bool useAnd = false)
		{
			string result;
			if (items == null)
			{
				result = "";
			}
			else
			{
				string text = null;
				string text2 = null;
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
					foreach (string text4 in items)
					{
						if (!text4.NullOrEmpty())
						{
							if (text2 == null)
							{
								text2 = text4;
							}
							if (text != null)
							{
								stringBuilder.Append(text + ", ");
							}
							text = text4;
							num++;
						}
					}
				}
				if (num == 0)
				{
					result = "NoneLower".Translate();
				}
				else if (num == 1)
				{
					result = text;
				}
				else if (useAnd)
				{
					if (num == 2)
					{
						result = string.Concat(new string[]
						{
							text2,
							" ",
							"AndLower".Translate(),
							" ",
							text
						});
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
			}
			return result;
		}

		public static string ToLineList(this IEnumerable<string> entries, string prefix = "")
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
			foreach (string value in entries)
			{
				if (!flag)
				{
					stringBuilder.Append(spacer);
				}
				stringBuilder.Append(value);
				flag = false;
			}
			return stringBuilder.ToString();
		}

		public static string ToCamelCase(string str)
		{
			str = GenText.ToTitleCaseSmart(str);
			return str.Replace(" ", null);
		}

		public static string Truncate(this string str, float width, Dictionary<string, string> cache = null)
		{
			if (cache != null)
			{
				string text;
				if (cache.TryGetValue(str, out text))
				{
					return text;
				}
			}
			string result;
			if (Text.CalcSize(str).x <= width)
			{
				if (cache != null)
				{
					cache.Add(str, str);
				}
				result = str;
			}
			else
			{
				string text = str;
				do
				{
					text = text.Substring(0, text.Length - 1);
				}
				while (text.Length > 0 && Text.CalcSize(text + "...").x > width);
				text += "...";
				if (cache != null)
				{
					cache.Add(str, text);
				}
				result = text;
			}
			return result;
		}

		public static string Flatten(this string str)
		{
			string result;
			if (str.NullOrEmpty())
			{
				result = str;
			}
			else
			{
				if (str.Contains("\n"))
				{
					str = str.Replace("\n", " ");
				}
				if (str.Contains("\r"))
				{
					str = str.Replace("\r", "");
				}
				str = str.MergeMultipleSpaces(false);
				result = str.Trim(new char[]
				{
					' ',
					'\n',
					'\r',
					'\t'
				});
			}
			return result;
		}

		public static string MergeMultipleSpaces(this string str, bool leaveMultipleSpacesAtLineBeginning = true)
		{
			string result;
			if (str.NullOrEmpty())
			{
				result = str;
			}
			else if (!str.Contains("  "))
			{
				result = str;
			}
			else
			{
				bool flag = true;
				GenText.tmpStringBuilder.Length = 0;
				for (int i = 0; i < str.Length; i++)
				{
					if (str[i] == '\r' || str[i] == '\n')
					{
						flag = true;
					}
					if ((leaveMultipleSpacesAtLineBeginning && flag) || str[i] != ' ' || i == 0 || str[i - 1] != ' ')
					{
						GenText.tmpStringBuilder.Append(str[i]);
					}
					if (!char.IsWhiteSpace(str[i]))
					{
						flag = false;
					}
				}
				result = GenText.tmpStringBuilder.ToString();
			}
			return result;
		}

		public static string TrimmedToLength(this string str, int length)
		{
			string result;
			if (str == null || str.Length <= length)
			{
				result = str;
			}
			else
			{
				result = str.Substring(0, length);
			}
			return result;
		}

		public static bool ContainsEmptyLines(string str)
		{
			return str.NullOrEmpty() || (str[0] == '\n' || str[0] == '\r') || (str[str.Length - 1] == '\n' || str[str.Length - 1] == '\r') || (str.Contains("\n\n") || str.Contains("\r\n\r\n") || str.Contains("\r\r"));
		}

		public static string ToStringByStyle(this float f, ToStringStyle style, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			if (style == ToStringStyle.Temperature && numberSense == ToStringNumberSense.Offset)
			{
				style = ToStringStyle.TemperatureOffset;
			}
			if (numberSense == ToStringNumberSense.Factor)
			{
				if (f >= 10f)
				{
					style = ToStringStyle.FloatMaxTwo;
				}
				else
				{
					style = ToStringStyle.PercentZero;
				}
			}
			string text;
			switch (style)
			{
			case ToStringStyle.Integer:
				text = Mathf.RoundToInt(f).ToString();
				break;
			case ToStringStyle.FloatOne:
				text = f.ToString("F1");
				break;
			case ToStringStyle.FloatTwo:
				text = f.ToString("F2");
				break;
			case ToStringStyle.FloatThree:
				text = f.ToString("F3");
				break;
			case ToStringStyle.FloatMaxOne:
				text = f.ToString("0.#");
				break;
			case ToStringStyle.FloatMaxTwo:
				text = f.ToString("0.##");
				break;
			case ToStringStyle.FloatMaxThree:
				text = f.ToString("0.###");
				break;
			case ToStringStyle.FloatTwoOrThree:
				text = f.ToString((f != 0f && Mathf.Abs(f) < 0.01f) ? "F3" : "F2");
				break;
			case ToStringStyle.PercentZero:
				text = f.ToStringPercent();
				break;
			case ToStringStyle.PercentOne:
				text = f.ToStringPercent("F1");
				break;
			case ToStringStyle.PercentTwo:
				text = f.ToStringPercent("F2");
				break;
			case ToStringStyle.Temperature:
				text = f.ToStringTemperature("F1");
				break;
			case ToStringStyle.TemperatureOffset:
				text = f.ToStringTemperatureOffset("F1");
				break;
			case ToStringStyle.WorkAmount:
				text = f.ToStringWorkAmount();
				break;
			default:
				Log.Error("Unknown ToStringStyle " + style, false);
				text = f.ToString();
				break;
			}
			if (numberSense == ToStringNumberSense.Offset)
			{
				if (f >= 0f)
				{
					text = "+" + text;
				}
			}
			else if (numberSense == ToStringNumberSense.Factor)
			{
				text = "x" + text;
			}
			return text;
		}

		public static string ToStringDecimalIfSmall(this float f)
		{
			string result;
			if (Mathf.Abs(f) < 1f)
			{
				result = Math.Round((double)f, 2).ToString("0.##");
			}
			else if (Mathf.Abs(f) < 10f)
			{
				result = Math.Round((double)f, 1).ToString("0.#");
			}
			else
			{
				result = Mathf.RoundToInt(f).ToStringCached();
			}
			return result;
		}

		public static string ToStringPercent(this float f)
		{
			return (f * 100f).ToStringDecimalIfSmall() + "%";
		}

		public static string ToStringPercent(this float f, string format)
		{
			return ((f + 1E-05f) * 100f).ToString(format) + "%";
		}

		public static string ToStringMoney(this float f, string format = null)
		{
			if (format == null)
			{
				if (f > 100f)
				{
					format = "F0";
				}
				else
				{
					format = "F2";
				}
			}
			return "$" + f.ToString(format);
		}

		public static string ToStringWithSign(this int i)
		{
			return i.ToString("+#;-#;0");
		}

		public static string ToStringWithSign(this float f, string format = "0.##")
		{
			string result;
			if (f > 0f)
			{
				result = "+" + f.ToString(format);
			}
			else
			{
				result = f.ToString(format);
			}
			return result;
		}

		public static string ToStringKilobytes(this int bytes, string format = "F2")
		{
			return ((float)bytes / 1024f).ToString(format) + "Kb";
		}

		public static string ToStringYesNo(this bool b)
		{
			return (!b) ? "No".Translate() : "Yes".Translate();
		}

		public static string ToStringLongitude(this float longitude)
		{
			bool flag = longitude < 0f;
			if (flag)
			{
				longitude = -longitude;
			}
			return longitude.ToString("F2") + '°' + ((!flag) ? "E" : "W");
		}

		public static string ToStringLatitude(this float latitude)
		{
			bool flag = latitude < 0f;
			if (flag)
			{
				latitude = -latitude;
			}
			return latitude.ToString("F2") + '°' + ((!flag) ? "N" : "S");
		}

		public static string ToStringMass(this float mass)
		{
			string result;
			if (mass == 0f)
			{
				result = "0 g";
			}
			else
			{
				float num = Mathf.Abs(mass);
				if (num >= 100f)
				{
					result = mass.ToString("F0") + " kg";
				}
				else if (num >= 10f)
				{
					result = mass.ToString("0.#") + " kg";
				}
				else if (num >= 0.1f)
				{
					result = mass.ToString("0.##") + " kg";
				}
				else
				{
					float num2 = mass * 1000f;
					if (num >= 0.01f)
					{
						result = num2.ToString("F0") + " g";
					}
					else if (num >= 0.001f)
					{
						result = num2.ToString("0.#") + " g";
					}
					else
					{
						result = num2.ToString("0.##") + " g";
					}
				}
			}
			return result;
		}

		public static string ToStringMassOffset(this float mass)
		{
			string text = mass.ToStringMass();
			string result;
			if (mass > 0f)
			{
				result = "+" + text;
			}
			else
			{
				result = text;
			}
			return result;
		}

		public static string ToStringSign(this float val)
		{
			string result;
			if (val >= 0f)
			{
				result = "+";
			}
			else
			{
				result = "";
			}
			return result;
		}

		public static string ToStringEnsureThreshold(this float value, float threshold, int decimalPlaces)
		{
			string result;
			if (value > threshold && Math.Round((double)value, decimalPlaces) <= Math.Round((double)threshold, decimalPlaces))
			{
				result = (value + 1f / Mathf.Pow(10f, (float)decimalPlaces)).ToString("F" + decimalPlaces);
			}
			else
			{
				result = value.ToString("F" + decimalPlaces);
			}
			return result;
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
			TemperatureDisplayMode temperatureMode = Prefs.TemperatureMode;
			string result;
			if (temperatureMode != TemperatureDisplayMode.Celsius)
			{
				if (temperatureMode != TemperatureDisplayMode.Fahrenheit)
				{
					if (temperatureMode != TemperatureDisplayMode.Kelvin)
					{
						throw new InvalidOperationException();
					}
					result = temp.ToString(format) + "K";
				}
				else
				{
					result = temp.ToString(format) + "F";
				}
			}
			else
			{
				result = temp.ToString(format) + "C";
			}
			return result;
		}

		public static string ToStringTwoDigits(this Vector2 v)
		{
			return string.Concat(new string[]
			{
				"(",
				v.x.ToString("F2"),
				", ",
				v.y.ToString("F2"),
				")"
			});
		}

		public static string ToStringWorkAmount(this float workAmount)
		{
			return Mathf.CeilToInt(workAmount / 60f).ToString();
		}

		public static string ToStringBytes(this int b, string format = "F2")
		{
			return ((float)b / 8f / 1024f).ToString(format) + "kb";
		}

		public static string ToStringBytes(this uint b, string format = "F2")
		{
			return (b / 8f / 1024f).ToString(format) + "kb";
		}

		public static string ToStringBytes(this long b, string format = "F2")
		{
			return ((float)b / 8f / 1024f).ToString(format) + "kb";
		}

		public static string ToStringBytes(this ulong b, string format = "F2")
		{
			return (b / 8f / 1024f).ToString(format) + "kb";
		}

		public static string ToStringReadable(this KeyCode k)
		{
			string result;
			switch (k)
			{
			case KeyCode.Keypad0:
				result = "Kp0";
				break;
			case KeyCode.Keypad1:
				result = "Kp1";
				break;
			case KeyCode.Keypad2:
				result = "Kp2";
				break;
			case KeyCode.Keypad3:
				result = "Kp3";
				break;
			case KeyCode.Keypad4:
				result = "Kp4";
				break;
			case KeyCode.Keypad5:
				result = "Kp5";
				break;
			case KeyCode.Keypad6:
				result = "Kp6";
				break;
			case KeyCode.Keypad7:
				result = "Kp7";
				break;
			case KeyCode.Keypad8:
				result = "Kp8";
				break;
			case KeyCode.Keypad9:
				result = "Kp9";
				break;
			case KeyCode.KeypadPeriod:
				result = "Kp.";
				break;
			case KeyCode.KeypadDivide:
				result = "Kp/";
				break;
			case KeyCode.KeypadMultiply:
				result = "Kp*";
				break;
			case KeyCode.KeypadMinus:
				result = "Kp-";
				break;
			case KeyCode.KeypadPlus:
				result = "Kp+";
				break;
			case KeyCode.KeypadEnter:
				result = "KpEnt";
				break;
			case KeyCode.KeypadEquals:
				result = "Kp=";
				break;
			case KeyCode.UpArrow:
				result = "Up";
				break;
			case KeyCode.DownArrow:
				result = "Down";
				break;
			case KeyCode.RightArrow:
				result = "Right";
				break;
			case KeyCode.LeftArrow:
				result = "Left";
				break;
			case KeyCode.Insert:
				result = "Ins";
				break;
			case KeyCode.Home:
				result = "Home";
				break;
			case KeyCode.End:
				result = "End";
				break;
			case KeyCode.PageUp:
				result = "PgUp";
				break;
			case KeyCode.PageDown:
				result = "PgDn";
				break;
			default:
				switch (k)
				{
				case KeyCode.Exclaim:
					result = "!";
					break;
				case KeyCode.DoubleQuote:
					result = "\"";
					break;
				case KeyCode.Hash:
					result = "#";
					break;
				case KeyCode.Dollar:
					result = "$";
					break;
				default:
					if (k != KeyCode.Clear)
					{
						if (k != KeyCode.Return)
						{
							if (k != KeyCode.Backspace)
							{
								if (k != KeyCode.Escape)
								{
									if (k != KeyCode.Delete)
									{
										result = k.ToString();
									}
									else
									{
										result = "Del";
									}
								}
								else
								{
									result = "Esc";
								}
							}
							else
							{
								result = "Bksp";
							}
						}
						else
						{
							result = "Ent";
						}
					}
					else
					{
						result = "Clr";
					}
					break;
				case KeyCode.Ampersand:
					result = "&";
					break;
				case KeyCode.Quote:
					result = "'";
					break;
				case KeyCode.LeftParen:
					result = "(";
					break;
				case KeyCode.RightParen:
					result = ")";
					break;
				case KeyCode.Asterisk:
					result = "*";
					break;
				case KeyCode.Plus:
					result = "+";
					break;
				case KeyCode.Comma:
					result = ",";
					break;
				case KeyCode.Minus:
					result = "-";
					break;
				case KeyCode.Period:
					result = ".";
					break;
				case KeyCode.Slash:
					result = "/";
					break;
				case KeyCode.Alpha0:
					result = "0";
					break;
				case KeyCode.Alpha1:
					result = "1";
					break;
				case KeyCode.Alpha2:
					result = "2";
					break;
				case KeyCode.Alpha3:
					result = "3";
					break;
				case KeyCode.Alpha4:
					result = "4";
					break;
				case KeyCode.Alpha5:
					result = "5";
					break;
				case KeyCode.Alpha6:
					result = "6";
					break;
				case KeyCode.Alpha7:
					result = "7";
					break;
				case KeyCode.Alpha8:
					result = "8";
					break;
				case KeyCode.Alpha9:
					result = "9";
					break;
				case KeyCode.Colon:
					result = ":";
					break;
				case KeyCode.Semicolon:
					result = ";";
					break;
				case KeyCode.Less:
					result = "<";
					break;
				case KeyCode.Greater:
					result = ">";
					break;
				case KeyCode.Question:
					result = "?";
					break;
				case KeyCode.At:
					result = "@";
					break;
				case KeyCode.LeftBracket:
					result = "[";
					break;
				case KeyCode.Backslash:
					result = "\\";
					break;
				case KeyCode.RightBracket:
					result = "]";
					break;
				case KeyCode.Caret:
					result = "^";
					break;
				case KeyCode.Underscore:
					result = "_";
					break;
				case KeyCode.BackQuote:
					result = "`";
					break;
				}
				break;
			case KeyCode.Numlock:
				result = "NumL";
				break;
			case KeyCode.CapsLock:
				result = "CapL";
				break;
			case KeyCode.ScrollLock:
				result = "ScrL";
				break;
			case KeyCode.RightShift:
				result = "RShf";
				break;
			case KeyCode.LeftShift:
				result = "LShf";
				break;
			case KeyCode.RightControl:
				result = "RCtrl";
				break;
			case KeyCode.LeftControl:
				result = "LCtrl";
				break;
			case KeyCode.RightAlt:
				result = "RAlt";
				break;
			case KeyCode.LeftAlt:
				result = "LAlt";
				break;
			case KeyCode.RightCommand:
				result = "Appl";
				break;
			case KeyCode.LeftCommand:
				result = "Cmd";
				break;
			case KeyCode.LeftWindows:
				result = "Win";
				break;
			case KeyCode.RightWindows:
				result = "Win";
				break;
			case KeyCode.AltGr:
				result = "AltGr";
				break;
			case KeyCode.Help:
				result = "Help";
				break;
			case KeyCode.Print:
				result = "Prnt";
				break;
			case KeyCode.SysReq:
				result = "SysReq";
				break;
			case KeyCode.Break:
				result = "Brk";
				break;
			case KeyCode.Menu:
				result = "Menu";
				break;
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

		public static string WordWrapAt(this string text, float length)
		{
			Text.Font = GameFont.Medium;
			string result;
			if (text.GetWidthCached() < length)
			{
				result = text;
			}
			else
			{
				IEnumerable<Pair<char, int>> source = from p in text.Select((char c, int idx) => new Pair<char, int>(c, idx))
				where p.First == ' '
				select p;
				if (!source.Any<Pair<char, int>>())
				{
					result = text;
				}
				else
				{
					Pair<char, int> pair = source.MinBy((Pair<char, int> p) => Mathf.Abs(text.Substring(0, p.Second).GetWidthCached() - text.Substring(p.Second + 1).GetWidthCached()));
					result = text.Substring(0, pair.Second) + "\n" + text.Substring(pair.Second + 1);
				}
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GenText()
		{
		}

		[CompilerGenerated]
		private static Pair<char, int> <WordWrapAt>m__0(char c, int idx)
		{
			return new Pair<char, int>(c, idx);
		}

		[CompilerGenerated]
		private static bool <WordWrapAt>m__1(Pair<char, int> p)
		{
			return p.First == ' ';
		}

		[CompilerGenerated]
		private sealed class <WithoutVowels>c__AnonStorey1
		{
			internal string vowels;

			public <WithoutVowels>c__AnonStorey1()
			{
			}

			internal bool <>m__0(char c)
			{
				return !this.vowels.Contains(c);
			}
		}

		[CompilerGenerated]
		private sealed class <LinesFromString>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal string[] <lineSeparators>__0;

			internal string text;

			internal string[] $locvar0;

			internal int $locvar1;

			internal string <str>__1;

			internal string <doneStr>__2;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <LinesFromString>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					lineSeparators = new string[]
					{
						"\r\n",
						"\n"
					};
					array = text.Split(lineSeparators, StringSplitOptions.RemoveEmptyEntries);
					i = 0;
					goto IL_108;
				case 1u:
					break;
				default:
					return false;
				}
				IL_FA:
				i++;
				IL_108:
				if (i >= array.Length)
				{
					this.$PC = -1;
				}
				else
				{
					str = array[i];
					doneStr = str.Trim();
					if (doneStr.StartsWith("//"))
					{
						goto IL_FA;
					}
					doneStr = doneStr.Split(new string[]
					{
						"//"
					}, StringSplitOptions.None)[0];
					if (doneStr.Length == 0)
					{
						goto IL_FA;
					}
					this.$current = doneStr;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				GenText.<LinesFromString>c__Iterator0 <LinesFromString>c__Iterator = new GenText.<LinesFromString>c__Iterator0();
				<LinesFromString>c__Iterator.text = text;
				return <LinesFromString>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <WordWrapAt>c__AnonStorey2
		{
			internal string text;

			public <WordWrapAt>c__AnonStorey2()
			{
			}

			internal float <>m__0(Pair<char, int> p)
			{
				return Mathf.Abs(this.text.Substring(0, p.Second).GetWidthCached() - this.text.Substring(p.Second + 1).GetWidthCached());
			}
		}
	}
}
