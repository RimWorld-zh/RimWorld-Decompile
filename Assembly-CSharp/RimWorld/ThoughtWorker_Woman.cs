using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Woman : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			return p.RaceProps.Humanlike ? (p.story.traits.HasTrait(TraitDefOf.DislikesWomen) ? (RelationsUtility.PawnsKnowEachOther(p, other) ? ((other.def == p.def) ? ((other.gender == Gender.Female) ? true : false) : false) : false) : false) : false;
		}
	}
}
