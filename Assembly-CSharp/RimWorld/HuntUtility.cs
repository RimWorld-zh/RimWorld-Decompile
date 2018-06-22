using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007CE RID: 1998
	public static class HuntUtility
	{
		// Token: 0x06002C55 RID: 11349 RVA: 0x00176398 File Offset: 0x00174798
		public static void ShowDesignationWarnings(Pawn pawn)
		{
			float baseManhunterOnDamageChance = PawnUtility.GetBaseManhunterOnDamageChance(pawn.kindDef);
			float manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn.kindDef);
			if (baseManhunterOnDamageChance > 0.015f)
			{
				string text = "MessageAnimalsGoPsychoHunted".Translate(new object[]
				{
					pawn.kindDef.GetLabelPlural(-1).CapitalizeFirst(),
					manhunterOnDamageChance.ToStringPercent()
				}).CapitalizeFirst();
				Messages.Message(text, pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
