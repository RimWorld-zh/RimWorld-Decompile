using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A8 RID: 1960
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x06002B55 RID: 11093 RVA: 0x0016DE98 File Offset: 0x0016C298
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002B56 RID: 11094 RVA: 0x0016DEC4 File Offset: 0x0016C2C4
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

		// Token: 0x06002B57 RID: 11095 RVA: 0x0016DEE8 File Offset: 0x0016C2E8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}
	}
}
