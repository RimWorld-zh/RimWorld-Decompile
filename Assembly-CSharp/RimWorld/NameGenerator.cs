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
		// Token: 0x0600370C RID: 14092 RVA: 0x001D641C File Offset: 0x001D481C
		public static string GenerateName(RulePackDef rootPack, IEnumerable<string> extantNames, bool appendNumberIfNameUsed = false, string rootKeyword = null)
		{
			return NameGenerator.GenerateName(rootPack, (string x) => !extantNames.Contains(x), appendNumberIfNameUsed, rootKeyword, null);
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x001D6454 File Offset: 0x001D4854
		public static string GenerateName(RulePackDef rootPack, Predicate<string> validator = null, bool appendNumberIfNameUsed = false, string rootKeyword = null, string testPawnNameSymbol = null)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(rootPack);
			if (testPawnNameSymbol != null)
			{
				request.Rules.Add(new Rule_String("ANYPAWN_nameDef", testPawnNameSymbol));
				request.Rules.Add(new Rule_String("ANYPAWN_nameIndef", testPawnNameSymbol));
			}
			rootKeyword = ((rootKeyword == null) ? rootPack.RulesPlusIncludes[0].keyword : rootKeyword);
			string result;
			if (appendNumberIfNameUsed)
			{
				for (int i = 0; i < 100; i++)
				{
					for (int j = 0; j < 5; j++)
					{
						string text = GenText.ToTitleCaseSmart(GrammarResolver.Resolve(rootKeyword, request, null, false));
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
				result = GenText.ToTitleCaseSmart(GrammarResolver.Resolve(rootKeyword, request, null, false));
			}
			else
			{
				for (int k = 0; k < 150; k++)
				{
					string text2 = GenText.ToTitleCaseSmart(GrammarResolver.Resolve(rootKeyword, request, null, false));
					if (validator == null || validator(text2))
					{
						return text2;
					}
				}
				Log.Error("Could not get new name (rule pack: " + rootPack + ")", false);
				result = "Errorname";
			}
			return result;
		}
	}
}
