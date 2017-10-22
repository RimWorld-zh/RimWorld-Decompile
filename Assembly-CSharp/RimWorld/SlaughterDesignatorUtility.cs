using Verse;

namespace RimWorld
{
	public static class SlaughterDesignatorUtility
	{
		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (designated.RaceProps.IsFlesh)
			{
				Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null);
				if (firstDirectRelationPawn != null)
				{
					Messages.Message("MessageSlaughteringBondedAnimal".Translate(designated.LabelShort, firstDirectRelationPawn.LabelShort), (Thing)designated, MessageSound.Standard);
				}
			}
		}
	}
}
