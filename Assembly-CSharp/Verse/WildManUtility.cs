using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000A89 RID: 2697
	public static class WildManUtility
	{
		// Token: 0x04002588 RID: 9608
		public const float ManhunterOnDamageChance = 0.5f;

		// Token: 0x04002589 RID: 9609
		public const float ManhunterOnTameFailChance = 0.1f;

		// Token: 0x06003BDC RID: 15324 RVA: 0x001F9148 File Offset: 0x001F7548
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x001F916C File Offset: 0x001F756C
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x001F919C File Offset: 0x001F759C
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}
	}
}
