using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BCE RID: 3022
	public static class CombatLogTester
	{
		// Token: 0x060041CC RID: 16844 RVA: 0x0022A38C File Offset: 0x0022878C
		public static Pawn GenerateRandom()
		{
			PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.RandomElementByWeight((PawnKindDef pawnkind) => (float)((!pawnkind.RaceProps.Humanlike) ? 1 : 5));
			Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
			return PawnGenerator.GeneratePawn(pawnKindDef, faction);
		}
	}
}
