using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A8 RID: 1960
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x06002B5E RID: 11102 RVA: 0x0016EA51 File Offset: 0x0016CE51
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002B5F RID: 11103 RVA: 0x0016EA7C File Offset: 0x0016CE7C
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

		// Token: 0x06002B60 RID: 11104 RVA: 0x0016EAA0 File Offset: 0x0016CEA0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}
	}
}
