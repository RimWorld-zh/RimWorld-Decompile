using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200098F RID: 2447
	public static class NameGenerator
	{
		// Token: 0x06003709 RID: 14089 RVA: 0x001D6A2C File Offset: 0x001D4E2C
		public static string GenerateName(RulePackDef rootPack, IEnumerable<string> extantNames, bool appendNumberIfNameUsed = false, string rootKeyword = null)
		{
			return NameGenerator.GenerateName(rootPack, (string x) => !extantNames.Contains(x), appendNumberIfNameUsed, rootKeyword, null);
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x001D6A64 File Offset: 0x001D4E64
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
