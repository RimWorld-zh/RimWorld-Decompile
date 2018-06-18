using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AC RID: 1964
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x06002B65 RID: 11109 RVA: 0x0016E879 File Offset: 0x0016CC79
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002B66 RID: 11110 RVA: 0x0016E8A4 File Offset: 0x0016CCA4
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

		// Token: 0x06002B67 RID: 11111 RVA: 0x0016E8C8 File Offset: 0x0016CCC8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}
	}
}
