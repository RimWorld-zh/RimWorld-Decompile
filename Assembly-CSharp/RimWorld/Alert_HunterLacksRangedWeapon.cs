using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079B RID: 1947
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		// Token: 0x06002B26 RID: 11046 RVA: 0x0016CAF4 File Offset: 0x0016AEF4
		public Alert_HunterLacksRangedWeapon()
		{
			this.defaultLabel = "HunterLacksWeapon".Translate();
			this.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002B27 RID: 11047 RVA: 0x0016CB24 File Offset: 0x0016AF24
		private IEnumerable<Pawn> HuntersWithoutRangedWeapon
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (p.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && !WorkGiver_HunterHunt.HasHuntingWeapon(p) && !p.Downed)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x0016CB48 File Offset: 0x0016AF48
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HuntersWithoutRangedWeapon);
		}
	}
}
