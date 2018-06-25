using System;
using Verse;

namespace RimWorld
{
	public static class HuntUtility
	{
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
