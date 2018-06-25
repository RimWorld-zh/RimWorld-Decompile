using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BCD RID: 3021
	public static class CombatLogTester
	{
		// Token: 0x060041D1 RID: 16849 RVA: 0x0022AE1C File Offset: 0x0022921C
		public static Pawn GenerateRandom()
		{
			PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.RandomElementByWeight((PawnKindDef pawnkind) => (float)((!pawnkind.RaceProps.Humanlike) ? 1 : 5));
			Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
			return PawnGenerator.GeneratePawn(pawnKindDef, faction);
		}
	}
}
