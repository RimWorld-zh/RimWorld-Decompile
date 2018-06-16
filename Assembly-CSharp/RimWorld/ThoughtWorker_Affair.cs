using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020001FA RID: 506
	public class ThoughtWorker_Affair : ThoughtWorker
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x00057CAC File Offset: 0x000560AC
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
					if (directRelations[i].otherPawn != p)
					{
						if (!directRelations[i].otherPawn.Dead)
						{
							if (directRelations[i].def == PawnRelationDefOf.Lover || directRelations[i].def == PawnRelationDefOf.Fiance)
							{
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}
	}
}
