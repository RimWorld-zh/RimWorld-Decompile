using System;

namespace RimWorld
{
	// Token: 0x02000204 RID: 516
	public class Thought_ChemicalInterestVsTeetotaler : Thought_SituationalSocial
	{
		// Token: 0x060009D4 RID: 2516 RVA: 0x000583EC File Offset: 0x000567EC
		public override float OpinionOffset()
		{
			int num = this.pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			int num2 = this.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			float result;
			if (num2 >= 0)
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
