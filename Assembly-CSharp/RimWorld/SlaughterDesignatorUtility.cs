using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DC RID: 2012
	public static class SlaughterDesignatorUtility
	{
		// Token: 0x06002C95 RID: 11413 RVA: 0x00177614 File Offset: 0x00175A14
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
