using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Incestuous : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			return other.RaceProps.Humanlike ? (RelationsUtility.PawnsKnowEachOther(pawn, other) ? ((LovePartnerRelationUtility.IncestOpinionOffsetFor(other, pawn) != 0.0) ? true : false) : false) : false;
		}
	}
}
