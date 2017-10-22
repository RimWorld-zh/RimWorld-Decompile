namespace RimWorld
{
	public class Thought_ChemicalInterestVsTeetotaler : Thought_SituationalSocial
	{
		public override float OpinionOffset()
		{
			int num = base.pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			int num2 = base.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			return (float)((num2 < 0) ? ((num != 1) ? -30.0 : -20.0) : 0.0);
		}
	}
}
