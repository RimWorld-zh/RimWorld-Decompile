using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public static class TaleTextGenerator
	{
		private const float TalelessChanceWithTales = 0.2f;

		public static string GenerateTextFromTale(TextGenerationPurpose purpose, Tale tale, int seed, List<Rule> extraRules)
		{
			Rand.PushState();
			Rand.Seed = seed;
			string text = (string)null;
			List<Rule> list = new List<Rule>();
			list.AddRange(extraRules);
			switch (purpose)
			{
			case TextGenerationPurpose.ArtDescription:
			{
				text = "art_description_root";
				if (tale != null && Rand.Value > 0.20000000298023224)
				{
					list.AddRange(RulePackDefOf.ArtDescriptionRoot_HasTale.Rules);
					list.AddRange(tale.GetTextGenerationRules());
				}
				else
				{
					list.AddRange(RulePackDefOf.ArtDescriptionRoot_Taleless.Rules);
					list.AddRange(RulePackDefOf.TalelessImages.Rules);
				}
				list.AddRange(RulePackDefOf.ArtDescriptionUtility_Global.Rules);
				break;
			}
			case TextGenerationPurpose.ArtName:
			{
				text = "art_name";
				if (tale != null)
				{
					list.AddRange(tale.GetTextGenerationRules());
				}
				break;
			}
			}
			string rootKeyword = text;
			List<Rule> rawRules = list;
			string debugLabel = (tale == null) ? "null_tale" : tale.def.defName;
			string result = GrammarResolver.Resolve(rootKeyword, rawRules, null, debugLabel);
			Rand.PopState();
			return result;
		}
	}
}
