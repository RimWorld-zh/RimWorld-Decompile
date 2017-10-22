using Verse;

namespace RimWorld
{
	public class StrengthWatcher
	{
		private Map map;

		public float StrengthRating
		{
			get
			{
				float num = 0f;
				foreach (Pawn freeColonist in this.map.mapPawns.FreeColonists)
				{
					float num2 = 1f;
					num2 *= freeColonist.health.summaryHealth.SummaryHealthPercent;
					if (freeColonist.Downed)
					{
						num2 = (float)(num2 * 0.30000001192092896);
					}
					num += num2;
				}
				foreach (Building allBuildingsColonistCombatTarget in this.map.listerBuildings.allBuildingsColonistCombatTargets)
				{
					if (allBuildingsColonistCombatTarget.def.building != null && allBuildingsColonistCombatTarget.def.building.IsTurret)
					{
						num = (float)(num + 0.30000001192092896);
					}
				}
				return num;
			}
		}

		public StrengthWatcher(Map map)
		{
			this.map = map;
		}
	}
}
