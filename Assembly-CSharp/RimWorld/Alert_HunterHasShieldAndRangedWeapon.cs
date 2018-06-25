using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AA RID: 1962
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x06002B62 RID: 11106 RVA: 0x0016EBA1 File Offset: 0x0016CFA1
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x0016EBCC File Offset: 0x0016CFCC
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

		// Token: 0x06002B64 RID: 11108 RVA: 0x0016EBF0 File Offset: 0x0016CFF0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}
	}
}
