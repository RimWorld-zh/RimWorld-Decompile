using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_WantToSleepWithSpouseOrLover : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false);
			return (directPawnRelation != null) ? ((!directPawnRelation.otherPawn.IsColonist || directPawnRelation.otherPawn.IsWorldPawn() || !directPawnRelation.otherPawn.relations.everSeenByPlayer) ? false : ((p.ownership.OwnedBed == null || p.ownership.OwnedBed != directPawnRelation.otherPawn.ownership.OwnedBed) ? ((p.relations.OpinionOf(directPawnRelation.otherPawn) > 0) ? true : false) : false)) : false;
		}
	}
}
