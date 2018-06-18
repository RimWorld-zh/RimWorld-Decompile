using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000A8D RID: 2701
	public static class WildManUtility
	{
		// Token: 0x06003BE1 RID: 15329 RVA: 0x001F8E28 File Offset: 0x001F7228
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x001F8E4C File Offset: 0x001F724C
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x001F8E7C File Offset: 0x001F727C
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
