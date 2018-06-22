using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000799 RID: 1945
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		// Token: 0x06002B22 RID: 11042 RVA: 0x0016C9A4 File Offset: 0x0016ADA4
		public Alert_HunterLacksRangedWeapon()
		{
			this.defaultLabel = "HunterLacksWeapon".Translate();
			this.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002B23 RID: 11043 RVA: 0x0016C9D4 File Offset: 0x0016ADD4
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

		// Token: 0x06002B24 RID: 11044 RVA: 0x0016C9F8 File Offset: 0x0016ADF8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HuntersWithoutRangedWeapon);
		}
	}
}
