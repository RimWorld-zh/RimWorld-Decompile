using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Affair : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			ThoughtState result;
			if (!p.relations.DirectRelationExists(PawnRelationDefOf.Spouse, otherPawn))
			{
				result = false;
			}
			else
			{
				List<DirectPawnRelation> directRelations = otherPawn.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (directRelations[i].otherPawn != p && !directRelations[i].otherPawn.Dead && (directRelations[i].def == PawnRelationDefOf.Lover || directRelations[i].def == PawnRelationDefOf.Fiance))
					{
						goto IL_0097;
					}
				}
				result = false;
			}
			goto IL_00c0;
			IL_0097:
			result = true;
			goto IL_00c0;
			IL_00c0:
			return result;
		}
	}
}
