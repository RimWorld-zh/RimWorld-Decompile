using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_OpinionOfMyLover : ThoughtWorker
	{
		public ThoughtWorker_OpinionOfMyLover()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false);
			ThoughtState result;
			if (directPawnRelation == null)
			{
				result = false;
			}
			else if (!directPawnRelation.otherPawn.IsColonist || directPawnRelation.otherPawn.IsWorldPawn() || !directPawnRelation.otherPawn.relations.everSeenByPlayer)
			{
				result = false;
			}
			else
			{
				result = (p.relations.OpinionOf(directPawnRelation.otherPawn) != 0);
			}
			return result;
		}
	}
}
