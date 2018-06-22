using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D8 RID: 2008
	public static class SlaughterDesignatorUtility
	{
		// Token: 0x06002C8E RID: 11406 RVA: 0x001777EC File Offset: 0x00175BEC
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
