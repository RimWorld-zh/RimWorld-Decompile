using System;

namespace RimWorld
{
	public class Thought_ChemicalInterestVsTeetotaler : Thought_SituationalSocial
	{
		public Thought_ChemicalInterestVsTeetotaler()
		{
		}

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
