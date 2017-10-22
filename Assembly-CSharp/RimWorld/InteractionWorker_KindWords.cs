using Verse;

namespace RimWorld
{
	public class InteractionWorker_KindWords : InteractionWorker
	{
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return (float)((!initiator.story.traits.HasTrait(TraitDefOf.Kind)) ? 0.0 : 0.0099999997764825821);
		}
	}
}
