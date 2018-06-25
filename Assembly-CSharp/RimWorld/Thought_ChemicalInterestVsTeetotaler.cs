using System;

namespace RimWorld
{
	// Token: 0x02000204 RID: 516
	public class Thought_ChemicalInterestVsTeetotaler : Thought_SituationalSocial
	{
		// Token: 0x060009D3 RID: 2515 RVA: 0x000583E8 File Offset: 0x000567E8
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
