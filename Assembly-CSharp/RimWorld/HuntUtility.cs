using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D0 RID: 2000
	public static class HuntUtility
	{
		// Token: 0x06002C59 RID: 11353 RVA: 0x001764E8 File Offset: 0x001748E8
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
