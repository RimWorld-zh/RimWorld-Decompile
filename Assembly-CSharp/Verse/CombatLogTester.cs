using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BCA RID: 3018
	public static class CombatLogTester
	{
		// Token: 0x060041CE RID: 16846 RVA: 0x0022AA60 File Offset: 0x00228E60
		public static Pawn GenerateRandom()
		{
			PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.RandomElementByWeight((PawnKindDef pawnkind) => (float)((!pawnkind.RaceProps.Humanlike) ? 1 : 5));
			Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
			return PawnGenerator.GeneratePawn(pawnKindDef, faction);
		}
	}
}
