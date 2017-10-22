namespace RimWorld
{
	public class Thought_TeetotalerVsChemicalInterest : Thought_SituationalSocial
	{
		public override float OpinionOffset()
		{
			int num = base.otherPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			return (float)((num > 0) ? ((num != 1) ? -30.0 : -20.0) : 0.0);
		}
	}
}
