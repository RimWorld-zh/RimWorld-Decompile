using Verse;

namespace RimWorld
{
	public class ThoughtWorker_SharedBed : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.RaceProps.Humanlike ? (LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(p) != null) : false;
		}
	}
}
