using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079D RID: 1949
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		// Token: 0x06002B27 RID: 11047 RVA: 0x0016C738 File Offset: 0x0016AB38
		public Alert_HunterLacksRangedWeapon()
		{
			this.defaultLabel = "HunterLacksWeapon".Translate();
			this.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x0016C768 File Offset: 0x0016AB68
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

		// Token: 0x06002B29 RID: 11049 RVA: 0x0016C78C File Offset: 0x0016AB8C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HuntersWithoutRangedWeapon);
		}
	}
}
