using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079D RID: 1949
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		// Token: 0x06002B29 RID: 11049 RVA: 0x0016C7CC File Offset: 0x0016ABCC
		public Alert_HunterLacksRangedWeapon()
		{
			this.defaultLabel = "HunterLacksWeapon".Translate();
			this.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x0016C7FC File Offset: 0x0016ABFC
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

		// Token: 0x06002B2B RID: 11051 RVA: 0x0016C820 File Offset: 0x0016AC20
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HuntersWithoutRangedWeapon);
		}
	}
}
