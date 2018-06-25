using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000443 RID: 1091
	public class StrengthWatcher
	{
		// Token: 0x04000B77 RID: 2935
		private Map map;

		// Token: 0x060012E7 RID: 4839 RVA: 0x000A3700 File Offset: 0x000A1B00
		public StrengthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x000A3710 File Offset: 0x000A1B10
		public float StrengthRating
		{
			get
			{
				float num = 0f;
				foreach (Pawn pawn in this.map.mapPawns.FreeColonists)
				{
					float num2 = 1f;
					num2 *= pawn.health.summaryHealth.SummaryHealthPercent;
					if (pawn.Downed)
					{
						num2 *= 0.3f;
					}
					num += num2;
				}
				foreach (Building building in this.map.listerBuildings.allBuildingsColonistCombatTargets)
				{
					if (building.def.building != null && building.def.building.IsTurret)
					{
						num += 0.3f;
					}
				}
				return num;
			}
		}
	}
}
