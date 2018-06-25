using System;
using RimWorld;

namespace Verse
{
	public static class WildManUtility
	{
		public const float ManhunterOnDamageChance = 0.5f;

		public const float ManhunterOnTameFailChance = 0.1f;

		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}
	}
}
