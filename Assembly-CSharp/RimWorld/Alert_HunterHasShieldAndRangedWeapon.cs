using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AA RID: 1962
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x06002B61 RID: 11105 RVA: 0x0016EE05 File Offset: 0x0016D205
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002B62 RID: 11106 RVA: 0x0016EE30 File Offset: 0x0016D230
		private IEnumerable<Pawn> BadHunters
		{
			get
			{
				foreach (Pawn col in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (col.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && WorkGiver_HunterHunt.HasShieldAndRangedWeapon(col))
					{
						yield return col;
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x0016EE54 File Offset: 0x0016D254
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}
	}
}
