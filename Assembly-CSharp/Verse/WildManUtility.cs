using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000A8B RID: 2699
	public static class WildManUtility
	{
		// Token: 0x04002589 RID: 9609
		public const float ManhunterOnDamageChance = 0.5f;

		// Token: 0x0400258A RID: 9610
		public const float ManhunterOnTameFailChance = 0.1f;

		// Token: 0x06003BE0 RID: 15328 RVA: 0x001F9274 File Offset: 0x001F7674
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x001F9298 File Offset: 0x001F7698
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x001F92C8 File Offset: 0x001F76C8
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}
	}
}
