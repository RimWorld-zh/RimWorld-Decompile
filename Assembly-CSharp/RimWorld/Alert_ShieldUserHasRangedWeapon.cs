using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A9 RID: 1961
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		// Token: 0x06002B56 RID: 11094 RVA: 0x0016E081 File Offset: 0x0016C481
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002B57 RID: 11095 RVA: 0x0016E0AC File Offset: 0x0016C4AC
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

		// Token: 0x06002B58 RID: 11096 RVA: 0x0016E0D0 File Offset: 0x0016C4D0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}
	}
}
