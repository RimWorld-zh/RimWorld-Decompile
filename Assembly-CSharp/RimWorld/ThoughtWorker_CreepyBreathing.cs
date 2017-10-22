using Verse;

namespace RimWorld
{
	public class ThoughtWorker_CreepyBreathing : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			return (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other)) ? false : (other.story.traits.HasTrait(TraitDefOf.CreepyBreathing) ? (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Hearing) ? true : false) : false);
		}
	}
}
