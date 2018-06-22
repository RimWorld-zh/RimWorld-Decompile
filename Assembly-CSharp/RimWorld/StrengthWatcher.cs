using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000441 RID: 1089
	public class StrengthWatcher
	{
		// Token: 0x060012E4 RID: 4836 RVA: 0x000A33B0 File Offset: 0x000A17B0
		public StrengthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x000A33C0 File Offset: 0x000A17C0
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

		// Token: 0x04000B74 RID: 2932
		private Map map;
	}
}
