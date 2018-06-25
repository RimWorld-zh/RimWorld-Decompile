using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079B RID: 1947
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		// Token: 0x06002B25 RID: 11045 RVA: 0x0016CD58 File Offset: 0x0016B158
		public Alert_HunterLacksRangedWeapon()
		{
			this.defaultLabel = "HunterLacksWeapon".Translate();
			this.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002B26 RID: 11046 RVA: 0x0016CD88 File Offset: 0x0016B188
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

		// Token: 0x06002B27 RID: 11047 RVA: 0x0016CDAC File Offset: 0x0016B1AC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HuntersWithoutRangedWeapon);
		}
	}
}
