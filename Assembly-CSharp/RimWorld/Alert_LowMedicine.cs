using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_LowMedicine : Alert
	{
		private const float MedicinePerColonistThreshold = 2f;

		public Alert_LowMedicine()
		{
			this.defaultLabel = "LowMedicine".Translate();
			this.defaultPriority = AlertPriority.High;
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
				if (num == 0)
				{
					result = string.Format("NoMedicineDesc".Translate(), new object[0]);
				}
				else
				{
					result = string.Format("LowMedicineDesc".Translate(), num);
				}
			}
			return result;
		}

		public override AlertReport GetReport()
		{
			AlertReport result;
			if (Find.TickManager.TicksGame < 150000)
			{
				result = false;
			}
			else
			{
				result = (this.MapWithLowMedicine() != null);
			}
			return result;
		}

		private Map MapWithLowMedicine()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					if (map.mapPawns.AnyColonistSpawned)
					{
						if ((float)this.MedicineCount(map) < 2f * (float)map.mapPawns.FreeColonistsSpawnedCount)
						{
							return map;
						}
					}
				}
			}
			return null;
		}

		private int MedicineCount(Map map)
		{
			return map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
		}
	}
}
