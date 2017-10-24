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
			GrammarRequest request = new GrammarRequest
			{
				Includes = 
				{
					rootPack
				}
			};
			string result;
			if (appendNumberIfNameUsed)
			{
				for (int i = 0; i < 100; i++)
				{
					int num = 0;
					while (num < 5)
					{
						text = GenText.ToTitleCaseSmart(GrammarResolver.Resolve((rootKeyword == null) ? rootPack.RulesPlusIncludes[0].keyword : rootKeyword, request, (string)null));
						if (i != 0)
						{
							text = text + " " + (i + 1);
						}
						if ((object)validator != null && !validator(text))
						{
							num++;
							continue;
						}
						goto IL_0085;
					}
				}
				result = GenText.ToTitleCaseSmart(GrammarResolver.Resolve((rootKeyword == null) ? rootPack.RulesPlusIncludes[0].keyword : rootKeyword, request, (string)null));
			}
			else
			{
				int num2 = 0;
				while (num2 < 150)
				{
					text = GenText.ToTitleCaseSmart(GrammarResolver.Resolve((rootKeyword == null) ? rootPack.RulesPlusIncludes[0].keyword : rootKeyword, request, (string)null));
					if ((object)validator != null && !validator(text))
					{
						num2++;
						continue;
					}
					goto IL_011c;
				}
				Log.Error("Could not get new name (rule pack: " + rootPack + ")");
				result = text;
			}
			goto IL_0155;
			IL_0155:
			return result;
			IL_011c:
			result = text;
			goto IL_0155;
			IL_0085:
			result = text;
			goto IL_0155;
		}
	}
}
