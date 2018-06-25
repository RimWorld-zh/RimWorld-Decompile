using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000665 RID: 1637
	public static class TaleTextGenerator
	{
		// Token: 0x0400137B RID: 4987
		private const float TalelessChanceWithTales = 0.2f;

		// Token: 0x06002241 RID: 8769 RVA: 0x00122E40 File Offset: 0x00121240
		public static string GenerateTextFromTale(TextGenerationPurpose purpose, Tale tale, int seed, RulePackDef extraInclude)
		{
			Rand.PushState();
			Rand.Seed = seed;
			string rootKeyword = null;
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(extraInclude);
			if (purpose == TextGenerationPurpose.ArtDescription)
			{
				rootKeyword = "r_art_description";
				if (tale != null && !Rand.Chance(0.2f))
				{
					request.Includes.Add(RulePackDefOf.ArtDescriptionRoot_HasTale);
					request.IncludesBare.AddRange(tale.GetTextGenerationIncludes());
					request.Rules.AddRange(tale.GetTextGenerationRules());
				}
				else
				{
					request.Includes.Add(RulePackDefOf.ArtDescriptionRoot_Taleless);
					request.Includes.Add(RulePackDefOf.TalelessImages);
				}
				request.Includes.Add(RulePackDefOf.ArtDescriptionUtility_Global);
			}
			else if (purpose == TextGenerationPurpose.ArtName)
			{
				rootKeyword = "r_art_name";
				if (tale != null)
				{
					request.IncludesBare.AddRange(tale.GetTextGenerationIncludes());
					request.Rules.AddRange(tale.GetTextGenerationRules());
				}
			}
			string result = GrammarResolver.Resolve(rootKeyword, request, (tale == null) ? "null_tale" : tale.def.defName, false);
			Rand.PopState();
			return result;
		}
	}
}
