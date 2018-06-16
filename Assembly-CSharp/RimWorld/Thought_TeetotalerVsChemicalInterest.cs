using System;

namespace RimWorld
{
	// Token: 0x02000202 RID: 514
	public class Thought_TeetotalerVsChemicalInterest : Thought_SituationalSocial
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x00058280 File Offset: 0x00056680
		public override float OpinionOffset()
		{
			int num = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			float result;
			if (num <= 0)
			{
				result = 0f;
			}
			else if (num == 1)
			{
				result = -20f;
			}
			else
			{
				result = -30f;
			}
			return result;
		}
	}
}
