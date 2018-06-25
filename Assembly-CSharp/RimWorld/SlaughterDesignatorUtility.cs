using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class SlaughterDesignatorUtility
	{
		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (designated.RaceProps.IsFlesh)
			{
				Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => !x.Dead);
				if (firstDirectRelationPawn != null)
				{
					Messages.Message("MessageSlaughteringBondedAnimal".Translate(new object[]
					{
						designated.LabelShort,
						firstDirectRelationPawn.LabelShort
					}), designated, MessageTypeDefOf.CautionInput, false);
				}
			}
		}

		[CompilerGenerated]
		private static bool <CheckWarnAboutBondedAnimal>m__0(Pawn x)
		{
			return !x.Dead;
		}
	}
}
