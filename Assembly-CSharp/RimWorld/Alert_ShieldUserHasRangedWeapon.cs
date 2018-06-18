using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A9 RID: 1961
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		// Token: 0x06002B58 RID: 11096 RVA: 0x0016E115 File Offset: 0x0016C515
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002B59 RID: 11097 RVA: 0x0016E140 File Offset: 0x0016C540
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

		// Token: 0x06002B5A RID: 11098 RVA: 0x0016E164 File Offset: 0x0016C564
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}
	}
}
