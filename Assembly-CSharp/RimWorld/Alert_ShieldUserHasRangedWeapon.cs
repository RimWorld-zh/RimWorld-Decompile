using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A7 RID: 1959
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		// Token: 0x06002B54 RID: 11092 RVA: 0x0016E6A1 File Offset: 0x0016CAA1
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002B55 RID: 11093 RVA: 0x0016E6CC File Offset: 0x0016CACC
		private IEnumerable<Pawn> ShieldUsersWithRangedWeapon
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon)
					{
						List<Apparel> ap = p.apparel.WornApparel;
						for (int i = 0; i < ap.Count; i++)
						{
							if (ap[i] is ShieldBelt)
							{
								yield return p;
								break;
							}
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x0016E6F0 File Offset: 0x0016CAF0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}
	}
}
