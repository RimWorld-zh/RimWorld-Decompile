using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000799 RID: 1945
	public class Alert_LowFood : Alert
	{
		// Token: 0x06002B0C RID: 11020 RVA: 0x0016B9D0 File Offset: 0x00169DD0
		public Alert_LowFood()
		{
			this.defaultLabel = "LowFood".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x0016B9F0 File Offset: 0x00169DF0
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

		// Token: 0x06002B0E RID: 11022 RVA: 0x0016BA9C File Offset: 0x00169E9C
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

		// Token: 0x06002B0F RID: 11023 RVA: 0x0016BAE4 File Offset: 0x00169EE4
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

		// Token: 0x0400172A RID: 5930
		private const float NutritionThresholdPerColonist = 4f;
	}
}
