using System;
using System.Collections.Generic;
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
				HashSet<Building>.Enumerator enumerator2 = this.map.listerBuildings.allBuildingsColonistCombatTargets.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Building current2 = enumerator2.Current;
						if (current2.def.building != null && current2.def.building.IsTurret)
						{
							num = (float)(num + 0.30000001192092896);
						}
					}
					return num;
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
			}
		}

		public StrengthWatcher(Map map)
		{
			this.map = map;
		}
	}
}
