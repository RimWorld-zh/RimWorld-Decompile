using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A4 RID: 1956
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x06002B4E RID: 11086 RVA: 0x0016E070 File Offset: 0x0016C470
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002B4F RID: 11087 RVA: 0x0016E09C File Offset: 0x0016C49C
		private IEnumerable<Pawn> BrawlersWithRangedWeapon
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (p.story.traits.HasTrait(TraitDefOf.Brawler) && p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x0016E0C0 File Offset: 0x0016C4C0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}
	}
}
