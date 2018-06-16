using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000A8D RID: 2701
	public static class WildManUtility
	{
		// Token: 0x06003BDF RID: 15327 RVA: 0x001F8D54 File Offset: 0x001F7154
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x001F8D78 File Offset: 0x001F7178
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x001F8DA8 File Offset: 0x001F71A8
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}

		// Token: 0x0400258D RID: 9613
		public const float ManhunterOnDamageChance = 0.5f;

		// Token: 0x0400258E RID: 9614
		public const float ManhunterOnTameFailChance = 0.1f;
	}
}
