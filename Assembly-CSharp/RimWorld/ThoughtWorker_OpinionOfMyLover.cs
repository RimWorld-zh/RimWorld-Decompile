using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_OpinionOfMyLover : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false);
			return (directPawnRelation != null) ? ((directPawnRelation.otherPawn.IsColonist && !directPawnRelation.otherPawn.IsWorldPawn() && directPawnRelation.otherPawn.relations.everSeenByPlayer) ? (p.relations.OpinionOf(directPawnRelation.otherPawn) != 0) : false) : false;
		}
	}
}
