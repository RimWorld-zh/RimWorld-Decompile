using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DA RID: 2010
	public static class SlaughterDesignatorUtility
	{
		// Token: 0x06002C91 RID: 11409 RVA: 0x00177BA0 File Offset: 0x00175FA0
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
	}
}
