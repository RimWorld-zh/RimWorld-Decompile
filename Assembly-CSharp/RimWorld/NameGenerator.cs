using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000991 RID: 2449
	public static class NameGenerator
	{
		// Token: 0x0600370A RID: 14090 RVA: 0x001D6354 File Offset: 0x001D4754
		public static string GenerateName(RulePackDef rootPack, IEnumerable<string> extantNames, bool appendNumberIfNameUsed = false, string rootKeyword = null)
		{
			return NameGenerator.GenerateName(rootPack, (string x) => !extantNames.Contains(x), appendNumberIfNameUsed, rootKeyword);
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x001D638C File Offset: 0x001D478C
		public static string GenerateName(RulePackDef rootPack, Predicate<string> validator = null, bool appendNumberIfNameUsed = false, string rootKeyword = null)
		{
			string text = null;
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(rootPack);
			string result;
			if (appendNumberIfNameUsed)
			{
				for (int i = 0; i < 100; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						text = GenText.ToTitleCaseSmart(GrammarResolver.Resolve((rootKeyword == null) ? rootPack.RulesPlusIncludes[0].keyword : rootKeyword, request, null, false));
						if (i != 0)
						{
							text = text + " " + (i + 1);
						}
						if (validator == null || validator(text))
						{
							return text;
						}
					}
				}
				result = GenText.ToTitleCaseSmart(GrammarResolver.Resolve((rootKeyword == null) ? rootPack.RulesPlusIncludes[0].keyword : rootKeyword, request, null, false));
			}
			else
			{
				for (int k = 0; k < 150; k++)
				{
					text = GenText.ToTitleCaseSmart(GrammarResolver.Resolve((rootKeyword == null) ? rootPack.RulesPlusIncludes[0].keyword : rootKeyword, request, null, false));
					if (validator == null || validator(text))
					{
						return text;
					}
				}
				Log.Error("Could not get new name (rule pack: " + rootPack + ")", false);
				result = text;
			}
			return result;
		}
	}
}
