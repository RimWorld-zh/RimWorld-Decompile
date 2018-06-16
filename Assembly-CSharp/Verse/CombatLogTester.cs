using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BCE RID: 3022
	public static class CombatLogTester
	{
		// Token: 0x060041CA RID: 16842 RVA: 0x0022A314 File Offset: 0x00228714
		public static Pawn GenerateRandom()
		{
			PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.RandomElementByWeight((PawnKindDef pawnkind) => (float)((!pawnkind.RaceProps.Humanlike) ? 1 : 5));
			Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
			return PawnGenerator.GeneratePawn(pawnKindDef, faction);
		}
	}
}
