using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020001F4 RID: 500
	public class ThoughtWorker_OpinionOfMyLover : ThoughtWorker
	{
		// Token: 0x060009B1 RID: 2481 RVA: 0x00057508 File Offset: 0x00055908
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
