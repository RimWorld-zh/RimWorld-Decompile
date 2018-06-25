using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_WantToSleepWithSpouseOrLover : ThoughtWorker
	{
		public ThoughtWorker_WantToSleepWithSpouseOrLover()
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
			else if (p.ownership.OwnedBed != null && p.ownership.OwnedBed == directPawnRelation.otherPawn.ownership.OwnedBed)
			{
				result = false;
			}
			else if (p.relations.OpinionOf(directPawnRelation.otherPawn) <= 0)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
