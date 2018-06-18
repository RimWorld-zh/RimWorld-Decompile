using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007D2 RID: 2002
	public static class HuntUtility
	{
		// Token: 0x06002C5C RID: 11356 RVA: 0x001761C0 File Offset: 0x001745C0
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
