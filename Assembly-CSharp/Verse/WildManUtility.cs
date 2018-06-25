using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000A8C RID: 2700
	public static class WildManUtility
	{
		// Token: 0x04002599 RID: 9625
		public const float ManhunterOnDamageChance = 0.5f;

		// Token: 0x0400259A RID: 9626
		public const float ManhunterOnTameFailChance = 0.1f;

		// Token: 0x06003BE1 RID: 15329 RVA: 0x001F95A0 File Offset: 0x001F79A0
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x001F95C4 File Offset: 0x001F79C4
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x001F95F4 File Offset: 0x001F79F4
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}
	}
}
