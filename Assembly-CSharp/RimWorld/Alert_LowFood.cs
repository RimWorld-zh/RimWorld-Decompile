using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Alert_LowFood : Alert
	{
		private const float NutritionThresholdPerColonist = 4f;

		public Alert_LowFood()
		{
			base.defaultLabel = "LowFood".Translate();
			base.defaultPriority = AlertPriority.High;
		}

		public override string GetExplanation()
		{
			Map map = this.MapWithLowFood();
			string result;
			if (map == null)
			{
				result = "";
			}
			else
			{
				float totalHumanEdibleNutrition = map.resourceCounter.TotalHumanEdibleNutrition;
				int num = map.mapPawns.FreeColonistsSpawnedCount + (from pr in map.mapPawns.PrisonersOfColony
				where pr.guest.GetsFood
				select pr).Count();
				int num2 = Mathf.FloorToInt(totalHumanEdibleNutrition / (float)num);
				result = string.Format("LowFoodDesc".Translate(), totalHumanEdibleNutrition.ToString("F0"), num.ToStringCached(), num2.ToStringCached());
			}
			return result;
		}

		public override AlertReport GetReport()
		{
			return (Find.TickManager.TicksGame >= 150000) ? (this.MapWithLowFood() != null) : false;
		}

		private Map MapWithLowFood()
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			Map result;
			while (true)
			{
				if (num < maps.Count)
				{
					Map map = maps[num];
					if (map.IsPlayerHome && map.mapPawns.AnyColonistSpawned)
					{
						int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
						if (map.resourceCounter.TotalHumanEdibleNutrition < 4.0 * (float)freeColonistsSpawnedCount)
						{
							result = map;
							break;
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
