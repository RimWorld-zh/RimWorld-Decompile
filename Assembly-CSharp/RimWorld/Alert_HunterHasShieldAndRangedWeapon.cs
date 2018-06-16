using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AC RID: 1964
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x06002B63 RID: 11107 RVA: 0x0016E7E5 File Offset: 0x0016CBE5
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002B64 RID: 11108 RVA: 0x0016E810 File Offset: 0x0016CC10
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

		// Token: 0x06002B65 RID: 11109 RVA: 0x0016E834 File Offset: 0x0016CC34
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}
	}
}
