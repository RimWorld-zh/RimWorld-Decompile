using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000F59 RID: 3929
	public static class GenText
	{
		// Token: 0x04003E62 RID: 15970
		private const int SaveNameMaxLength = 30;

		// Token: 0x04003E63 RID: 15971
		private const char DegreeSymbol = '°';

		// Token: 0x04003E64 RID: 15972
		private static StringBuilder tmpSb = new StringBuilder();

		// Token: 0x04003E65 RID: 15973
		private static StringBuilder tmpStringBuilder = new StringBuilder();

		// Token: 0x06005EE4 RID: 24292 RVA: 0x00305E78 File Offset: 0x00304278
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

		// Token: 0x06005EE5 RID: 24293 RVA: 0x00305EB4 File Offset: 0x003042B4
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

		// Token: 0x06005EE6 RID: 24294 RVA: 0x00305EF0 File Offset: 0x003042F0
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

		// Token: 0x06005EE7 RID: 24295 RVA: 0x00305F2C File Offset: 0x0030432C
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

		// Token: 0x06005EE8 RID: 24296 RVA: 0x00305F68 File Offset: 0x00304368
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

		// Token: 0x06005EE9 RID: 24297 RVA: 0x00305FA4 File Offset: 0x003043A4
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

		// Token: 0x06005EEA RID: 24298 RVA: 0x00305FE0 File Offset: 0x003043E0
		public static string AdjustedFor(this string text, Pawn p, string pawnSymbol = "PAWN")
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.DynamicWrapper);
			request.Rules.Add(new Rule_String("RULE", text));
			request.Rules.AddRange(GrammarUtility.RulesForPawn(pawnSymbol, p, null));
			return GrammarResolver.Resolve("r_root", request, null, false);
		}

		// Token: 0x06005EEB RID: 24299 RVA: 0x00306048 File Offset: 0x00304448
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

		// Token: 0x06005EEC RID: 24300 RVA: 0x003061EC File Offset: 0x003045EC
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

		// Token: 0x06005EED RID: 24301 RVA: 0x00306230 File Offset: 0x00304630
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

		// Token: 0x06005EEE RID: 24302 RVA: 0x00306274 File Offset: 0x00304674
		public static string KindLabelIndefinite(this Pawn pawn)
		{
			return Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(pawn.KindLabel);
		}

		// Token: 0x06005EEF RID: 24303 RVA: 0x0030629C File Offset: 0x0030469C
		public static string KindLabelDefinite(this Pawn pawn)
		{
			return Find.ActiveLanguageWorker.WithDefiniteArticlePostProcessed(pawn.KindLabel);
		}

		// Token: 0x06005EF0 RID: 24304 RVA: 0x003062C4 File Offset: 0x003046C4
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

		// Token: 0x06005EF1 RID: 24305 RVA: 0x00306304 File Offset: 0x00304704
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

		// Token: 0x06005EF2 RID: 24306 RVA: 0x003063D0 File Offset: 0x003047D0
		private static string WithoutVowels(this string s)
		{
			string vowels = "aeiouy";
			return new string((from c in s
			where !vowels.Contains(c)
			select c).ToArray<char>());
		}

		// Token: 0x06005EF3 RID: 24307 RVA: 0x00306414 File Offset: 0x00304814
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

		// Token: 0x06005EF4 RID: 24308 RVA: 0x00306474 File Offset: 0x00304874
		public static void SetTextSizeToFit(string text, Rect r)
		{
			Text.Font = GameFont.Small;
			float num = Text.CalcHeight(text, r.width);
			if (num > r.height)
			{
				Text.Font = GameFont.Tiny;
			}
		}

		// Token: 0x06005EF5 RID: 24309 RVA: 0x003064AC File Offset: 0x003048AC
		public static string TrimEndNewlines(this string s)
		{
			return s.TrimEnd(new char[]
			{
				'\r',
				'\n'
			});
		}

		// Token: 0x06005EF6 RID: 24310 RVA: 0x003064D8 File Offset: 0x003048D8
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

		// Token: 0x06005EF7 RID: 24311 RVA: 0x0030652C File Offset: 0x0030492C
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

		// Token: 0x06005EF8 RID: 24312 RVA: 0x00306574 File Offset: 0x00304974
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

		// Token: 0x06005EF9 RID: 24313 RVA: 0x003065C4 File Offset: 0x003049C4
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

		// Token: 0x06005EFA RID: 24314 RVA: 0x00306648 File Offset: 0x00304A48
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

		// Token: 0x06005EFB RID: 24315 RVA: 0x00306674 File Offset: 0x00304A74
		public static string GetInvalidFilenameCharacters()
		{
			return new string(Path.GetInvalidFileNameChars()) + "/\\{}<>:*|!@#$%^&*?";
		}

		// Token: 0x06005EFC RID: 24316 RVA: 0x003066A0 File Offset: 0x00304AA0
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

		// Token: 0x06005EFD RID: 24317 RVA: 0x003066F4 File Offset: 0x00304AF4
		public static string SanitizeFilename(string str)
		{
			return string.Join("_", str.Split(GenText.GetInvalidFilenameCharacters().ToArray<char>(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd(new char[]
			{
				'.'
			});
		}

		// Token: 0x06005EFE RID: 24318 RVA: 0x00306734 File Offset: 0x00304B34
		public static bool NullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		// Token: 0x06005EFF RID: 24319 RVA: 0x00306750 File Offset: 0x00304B50
		public static string SplitCamelCase(string Str)
		{
			return Regex.Replace(Str, "(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", " ${a}");
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x00306778 File Offset: 0x00304B78
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

		// Token: 0x06005F01 RID: 24321 RVA: 0x00306808 File Offset: 0x00304C08
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

		// Token: 0x06005F02 RID: 24322 RVA: 0x00306870 File Offset: 0x00304C70
		public static bool EqualsIgnoreCase(this string A, string B)
		{
			return string.Compare(A, B, true) == 0;
		}

		// Token: 0x06005F03 RID: 24323 RVA: 0x00306890 File Offset: 0x00304C90
		public static string WithoutByteOrderMark(this string str)
		{
			return str.Trim().Trim(new char[]
			{
				'﻿'
			});
		}

		// Token: 0x06005F04 RID: 24324 RVA: 0x003068C0 File Offset: 0x00304CC0
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

		// Token: 0x06005F05 RID: 24325 RVA: 0x00306954 File Offset: 0x00304D54
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

		// Token: 0x06005F06 RID: 24326 RVA: 0x003069C8 File Offset: 0x00304DC8
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

		// Token: 0x06005F07 RID: 24327 RVA: 0x00306A3C File Offset: 0x00304E3C
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

		// Token: 0x06005F08 RID: 24328 RVA: 0x00306AE0 File Offset: 0x00304EE0
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

		// Token: 0x06005F09 RID: 24329 RVA: 0x00306B98 File Offset: 0x00304F98
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

		// Token: 0x06005F0A RID: 24330 RVA: 0x00306C8C File Offset: 0x0030508C
		public static string CapitalizeAsTitle(string str)
		{
			return Find.ActiveLanguageWorker.ToTitleCase(str);
		}

		// Token: 0x06005F0B RID: 24331 RVA: 0x00306CAC File Offset: 0x003050AC
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

		// Token: 0x06005F0C RID: 24332 RVA: 0x00306E80 File Offset: 0x00305280
		public static string ToLineList(this IEnumerable<string> entries, string prefix = "")
		{
			return GenText.ToTextList(entries, "\n" + prefix);
		}

		// Token: 0x06005F0D RID: 24333 RVA: 0x00306EA8 File Offset: 0x003052A8
		public static string ToSpaceList(IEnumerable<string> entries)
		{
			return GenText.ToTextList(entries, " ");
		}

		// Token: 0x06005F0E RID: 24334 RVA: 0x00306EC8 File Offset: 0x003052C8
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

		// Token: 0x06005F0F RID: 24335 RVA: 0x00306F48 File Offset: 0x00305348
		public static string ToCamelCase(string str)
		{
			str = GenText.ToTitleCaseSmart(str);
			return str.Replace(" ", null);
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x00306F74 File Offset: 0x00305374
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

		// Token: 0x06005F11 RID: 24337 RVA: 0x0030702C File Offset: 0x0030542C
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

		// Token: 0x06005F12 RID: 24338 RVA: 0x003070B8 File Offset: 0x003054B8
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

		// Token: 0x06005F13 RID: 24339 RVA: 0x0030719C File Offset: 0x0030559C
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

		// Token: 0x06005F14 RID: 24340 RVA: 0x003071D4 File Offset: 0x003055D4
		public static bool ContainsEmptyLines(string str)
		{
			return str.NullOrEmpty() || (str[0] == '\n' || str[0] == '\r') || (str[str.Length - 1] == '\n' || str[str.Length - 1] == '\r') || (str.Contains("\n\n") || str.Contains("\r\n\r\n") || str.Contains("\r\r"));
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x00307288 File Offset: 0x00305688
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

		// Token: 0x06005F16 RID: 24342 RVA: 0x00307490 File Offset: 0x00305890
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

		// Token: 0x06005F17 RID: 24343 RVA: 0x00307508 File Offset: 0x00305908
		public static string ToStringPercent(this float f)
		{
			return (f * 100f).ToStringDecimalIfSmall() + "%";
		}

		// Token: 0x06005F18 RID: 24344 RVA: 0x00307534 File Offset: 0x00305934
		public static string ToStringPercent(this float f, string format)
		{
			return ((f + 1E-05f) * 100f).ToString(format) + "%";
		}

		// Token: 0x06005F19 RID: 24345 RVA: 0x0030756C File Offset: 0x0030596C
		public static string ToStringMoney(this float f)
		{
			return "$" + f.ToString("F2");
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x00307598 File Offset: 0x00305998
		public static string ToStringWithSign(this int i)
		{
			return i.ToString("+#;-#;0");
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x003075BC File Offset: 0x003059BC
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

		// Token: 0x06005F1C RID: 24348 RVA: 0x003075FC File Offset: 0x003059FC
		public static string ToStringKilobytes(this int bytes, string format = "F2")
		{
			return ((float)bytes / 1024f).ToString(format) + "Kb";
		}

		// Token: 0x06005F1D RID: 24349 RVA: 0x0030762C File Offset: 0x00305A2C
		public static string ToStringYesNo(this bool b)
		{
			return (!b) ? "No".Translate() : "Yes".Translate();
		}

		// Token: 0x06005F1E RID: 24350 RVA: 0x00307660 File Offset: 0x00305A60
		public static string ToStringLongitude(this float longitude)
		{
			bool flag = longitude < 0f;
			if (flag)
			{
				longitude = -longitude;
			}
			return longitude.ToString("F2") + '°' + ((!flag) ? "E" : "W");
		}

		// Token: 0x06005F1F RID: 24351 RVA: 0x003076B8 File Offset: 0x00305AB8
		public static string ToStringLatitude(this float latitude)
		{
			bool flag = latitude < 0f;
			if (flag)
			{
				latitude = -latitude;
			}
			return latitude.ToString("F2") + '°' + ((!flag) ? "N" : "S");
		}

		// Token: 0x06005F20 RID: 24352 RVA: 0x00307710 File Offset: 0x00305B10
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

		// Token: 0x06005F21 RID: 24353 RVA: 0x00307824 File Offset: 0x00305C24
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

		// Token: 0x06005F22 RID: 24354 RVA: 0x00307860 File Offset: 0x00305C60
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

		// Token: 0x06005F23 RID: 24355 RVA: 0x00307890 File Offset: 0x00305C90
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

		// Token: 0x06005F24 RID: 24356 RVA: 0x0030790C File Offset: 0x00305D0C
		public static string ToStringTemperature(this float celsiusTemp, string format = "F1")
		{
			celsiusTemp = GenTemperature.CelsiusTo(celsiusTemp, Prefs.TemperatureMode);
			return celsiusTemp.ToStringTemperatureRaw(format);
		}

		// Token: 0x06005F25 RID: 24357 RVA: 0x00307938 File Offset: 0x00305D38
		public static string ToStringTemperatureOffset(this float celsiusTemp, string format = "F1")
		{
			celsiusTemp = GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode);
			return celsiusTemp.ToStringTemperatureRaw(format);
		}

		// Token: 0x06005F26 RID: 24358 RVA: 0x00307964 File Offset: 0x00305D64
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

		// Token: 0x06005F27 RID: 24359 RVA: 0x003079E0 File Offset: 0x00305DE0
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

		// Token: 0x06005F28 RID: 24360 RVA: 0x00307A40 File Offset: 0x00305E40
		public static string ToStringWorkAmount(this float workAmount)
		{
			return Mathf.CeilToInt(workAmount / 60f).ToString();
		}

		// Token: 0x06005F29 RID: 24361 RVA: 0x00307A70 File Offset: 0x00305E70
		public static string ToStringBytes(this int b, string format = "F2")
		{
			return ((float)b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x06005F2A RID: 24362 RVA: 0x00307AA8 File Offset: 0x00305EA8
		public static string ToStringBytes(this uint b, string format = "F2")
		{
			return (b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x06005F2B RID: 24363 RVA: 0x00307AE0 File Offset: 0x00305EE0
		public static string ToStringBytes(this long b, string format = "F2")
		{
			return ((float)b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x06005F2C RID: 24364 RVA: 0x00307B18 File Offset: 0x00305F18
		public static string ToStringBytes(this ulong b, string format = "F2")
		{
			return (b / 8f / 1024f).ToString(format) + "kb";
		}

		// Token: 0x06005F2D RID: 24365 RVA: 0x00307B50 File Offset: 0x00305F50
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

		// Token: 0x06005F2E RID: 24366 RVA: 0x00308165 File Offset: 0x00306565
		public static void AppendWithComma(this StringBuilder sb, string text)
		{
			sb.AppendWithSeparator(text, ", ");
		}

		// Token: 0x06005F2F RID: 24367 RVA: 0x00308174 File Offset: 0x00306574
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

		// Token: 0x06005F30 RID: 24368 RVA: 0x003081A4 File Offset: 0x003065A4
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
	}
}
