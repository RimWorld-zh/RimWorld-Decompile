using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_LowMedicine : Alert
	{
		private const float MedicinePerColonistThreshold = 2f;

		public Alert_LowMedicine()
		{
			base.defaultLabel = "LowMedicine".Translate();
			base.defaultPriority = AlertPriority.High;
		}

		public override string GetExplanation()
		{
			Map map = this.MapWithLowMedicine();
			string result;
			if (map == null)
			{
				result = "";
			}
			else
			{
				int num = this.MedicineCount(map);
				result = ((num != 0) ? string.Format("LowMedicineDesc".Translate(), num) : string.Format("NoMedicineDesc".Translate()));
			}
			return result;
		}

		public override AlertReport GetReport()
		{
			return (Find.TickManager.TicksGame >= 150000) ? (this.MapWithLowMedicine() != null) : false;
		}

		private Map MapWithLowMedicine()
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			Map result;
			while (true)
			{
				if (num < maps.Count)
				{
					Map map = maps[num];
					if (map.IsPlayerHome && map.mapPawns.AnyColonistSpawned && (float)this.MedicineCount(map) < 2.0 * (float)map.mapPawns.FreeColonistsSpawnedCount)
					{
						result = map;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		private int MedicineCount(Map map)
		{
			return map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
		}
	}
}
