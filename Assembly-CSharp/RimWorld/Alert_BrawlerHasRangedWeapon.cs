using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A8 RID: 1960
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x06002B53 RID: 11091 RVA: 0x0016DE04 File Offset: 0x0016C204
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002B54 RID: 11092 RVA: 0x0016DE30 File Offset: 0x0016C230
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

		// Token: 0x06002B55 RID: 11093 RVA: 0x0016DE54 File Offset: 0x0016C254
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}
	}
}
