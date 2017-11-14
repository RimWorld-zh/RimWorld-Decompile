using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Ugly : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (other.RaceProps.Humanlike && RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
				{
					return false;
				}
				switch (other.story.traits.DegreeOfTrait(TraitDefOf.Beauty))
				{
				case -1:
					return ThoughtState.ActiveAtStage(0);
				case -2:
					return ThoughtState.ActiveAtStage(1);
				default:
					return false;
				}
			}
			return false;
		}
	}
}
