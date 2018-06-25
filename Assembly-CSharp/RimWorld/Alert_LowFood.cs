using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Alert_LowFood : Alert
	{
		private const float NutritionThresholdPerColonist = 4f;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public Alert_LowFood()
		{
			this.defaultLabel = "LowFood".Translate();
			this.defaultPriority = AlertPriority.High;
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
				select pr).Count<Pawn>();
				int num2 = Mathf.FloorToInt(totalHumanEdibleNutrition / (float)num);
				result = string.Format("LowFoodDesc".Translate(), totalHumanEdibleNutrition.ToString("F0"), num.ToStringCached(), num2.ToStringCached());
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
				result = (this.MapWithLowFood() != null);
			}
			return result;
		}

		private Map MapWithLowFood()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					if (map.mapPawns.AnyColonistSpawned)
					{
						int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
						if (map.resourceCounter.TotalHumanEdibleNutrition < 4f * (float)freeColonistsSpawnedCount)
						{
							return map;
						}
					}
				}
			}
			return null;
		}

		[CompilerGenerated]
		private static bool <GetExplanation>m__0(Pawn pr)
		{
			return pr.guest.GetsFood;
		}
	}
}
