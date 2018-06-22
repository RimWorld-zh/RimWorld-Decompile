using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A5 RID: 1957
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		// Token: 0x06002B51 RID: 11089 RVA: 0x0016E2ED File Offset: 0x0016C6ED
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x0016E318 File Offset: 0x0016C718
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

		// Token: 0x06002B53 RID: 11091 RVA: 0x0016E33C File Offset: 0x0016C73C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}
	}
}
