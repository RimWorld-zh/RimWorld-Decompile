using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A6 RID: 1958
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x06002B52 RID: 11090 RVA: 0x0016E1C0 File Offset: 0x0016C5C0
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002B53 RID: 11091 RVA: 0x0016E1EC File Offset: 0x0016C5EC
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

		// Token: 0x06002B54 RID: 11092 RVA: 0x0016E210 File Offset: 0x0016C610
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}
	}
}
