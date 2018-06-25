using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Affair : ThoughtWorker
	{
		public ThoughtWorker_Affair()
		{
		}

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
