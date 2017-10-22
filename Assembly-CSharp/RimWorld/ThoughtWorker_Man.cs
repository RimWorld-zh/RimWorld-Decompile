using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Man : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			return p.RaceProps.Humanlike ? (p.story.traits.HasTrait(TraitDefOf.DislikesMen) ? (RelationsUtility.PawnsKnowEachOther(p, other) ? ((other.def == p.def) ? ((other.gender == Gender.Male) ? true : false) : false) : false) : false) : false;
		}
	}
}
