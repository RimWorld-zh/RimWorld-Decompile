using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public static class NameGenerator
	{
		public static string GenerateName(RulePackDef rootPack, IEnumerable<string> extantNames, bool appendNumberIfNameUsed = false)
		{
			return NameGenerator.GenerateName(rootPack, (Predicate<string>)((string x) => !extantNames.Contains(x)), appendNumberIfNameUsed);
		}

		public static string GenerateName(RulePackDef rootPack, Predicate<string> validator = null, bool appendNumberIfNameUsed = false)
		{
			string text = (string)null;
			if (appendNumberIfNameUsed)
			{
				for (int i = 0; i < 100; i++)
				{
					int num = 0;
					while (num < 5)
					{
						text = GenText.ToTitleCaseSmart(GrammarResolver.Resolve(rootPack.Rules[0].keyword, rootPack.Rules, (string)null));
						if (i != 0)
						{
							text = text + " " + (i + 1);
						}
						if ((object)validator != null && !validator(text))
						{
							num++;
							continue;
						}
						return text;
					}
				}
				return GenText.ToTitleCaseSmart(GrammarResolver.Resolve(rootPack.Rules[0].keyword, rootPack.Rules, (string)null));
			}
			int num2 = 0;
			while (num2 < 150)
			{
				text = GenText.ToTitleCaseSmart(GrammarResolver.Resolve(rootPack.Rules[0].keyword, rootPack.Rules, (string)null));
				if ((object)validator != null && !validator(text))
				{
					num2++;
					continue;
				}
				return text;
			}
			Log.Error("Could not get new name.");
			return text;
		}
	}
}
