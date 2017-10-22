using System;
using Verse;

namespace RimWorld
{
	public static class SlaughterDesignatorUtility
	{
		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (designated.RaceProps.IsFlesh)
			{
				Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Predicate<Pawn>)((Pawn x) => !x.Dead));
				if (firstDirectRelationPawn != null)
				{
					Messages.Message("MessageSlaughteringBondedAnimal".Translate(designated.LabelShort, firstDirectRelationPawn.LabelShort), (Thing)designated, MessageTypeDefOf.CautionInput);
				}
			}
		}
	}
}
