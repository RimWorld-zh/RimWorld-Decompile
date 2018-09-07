using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	public static class CombatLogTester
	{
		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache0;

		public static Pawn GenerateRandom()
		{
			PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.RandomElementByWeight((PawnKindDef pawnkind) => (float)((!pawnkind.RaceProps.Humanlike) ? 1 : 5));
			Faction faction = FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType);
			return PawnGenerator.GeneratePawn(pawnKindDef, faction);
		}

		[CompilerGenerated]
		private static float <GenerateRandom>m__0(PawnKindDef pawnkind)
		{
			return (float)((!pawnkind.RaceProps.Humanlike) ? 1 : 5);
		}
	}
}
