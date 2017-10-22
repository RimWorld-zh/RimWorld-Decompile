using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public static class NameGenerator
	{
		public static string GenerateName(RulePackDef rootPack, IEnumerable<string> extantNames, bool appendNumberIfNameUsed = false, string rootKeyword = null)
		{
			return NameGenerator.GenerateName(rootPack, (Predicate<string>)((string x) => !extantNames.Contains(x)), appendNumberIfNameUsed, rootKeyword);
		}

		public static string GenerateName(RulePackDef rootPack, Predicate<string> validator = null, bool appendNumberIfNameUsed = false, string rootKeyword = null)
		{
			string text = (string)null;
			string result;
			if (appendNumberIfNameUsed)
			{
				for (int i = 0; i < 100; i++)
				{
					int num = 0;
					while (num < 5)
					{
						text = GenText.ToTitleCaseSmart(GrammarResolver.Resolve((rootKeyword == null) ? rootPack.Rules[0].keyword : rootKeyword, rootPack.Rules, null, (string)null));
						if (i != 0)
						{
							text = text + " " + (i + 1);
						}
						if ((object)validator != null && !validator(text))
						{
							num++;
							continue;
						}
						goto IL_0076;
					}
				}
				result = GenText.ToTitleCaseSmart(GrammarResolver.Resolve((rootKeyword == null) ? rootPack.Rules[0].keyword : rootKeyword, rootPack.Rules, null, (string)null));
			}
			else
			{
				int num2 = 0;
				while (num2 < 150)
				{
					text = GenText.ToTitleCaseSmart(GrammarResolver.Resolve((rootKeyword == null) ? rootPack.Rules[0].keyword : rootKeyword, rootPack.Rules, null, (string)null));
					if ((object)validator != null && !validator(text))
					{
						num2++;
						continue;
					}
					goto IL_0117;
				}
				Log.Error("Could not get new name (rule pack: " + rootPack + ")");
				result = text;
			}
			goto IL_014e;
			IL_014e:
			return result;
			IL_0117:
			result = text;
			goto IL_014e;
			IL_0076:
			result = text;
			goto IL_014e;
		}
	}
}
