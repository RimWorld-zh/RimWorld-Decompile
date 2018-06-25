using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A6 RID: 1958
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x06002B51 RID: 11089 RVA: 0x0016E424 File Offset: 0x0016C824
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x0016E450 File Offset: 0x0016C850
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

		// Token: 0x06002B53 RID: 11091 RVA: 0x0016E474 File Offset: 0x0016C874
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}
	}
}
