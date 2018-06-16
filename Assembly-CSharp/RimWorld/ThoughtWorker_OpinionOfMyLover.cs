using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020001F4 RID: 500
	public class ThoughtWorker_OpinionOfMyLover : ThoughtWorker
	{
		// Token: 0x060009B3 RID: 2483 RVA: 0x000574C4 File Offset: 0x000558C4
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
