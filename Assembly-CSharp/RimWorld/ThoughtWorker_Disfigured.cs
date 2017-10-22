using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Disfigured : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			return (!other.RaceProps.Humanlike || other.Dead) ? false : (RelationsUtility.PawnsKnowEachOther(pawn, other) ? (RelationsUtility.IsDisfigured(other) ? (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) ? true : false) : false) : false);
		}
	}
}
