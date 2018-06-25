using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A1 RID: 1953
	public class Alert_NeedMealSource : Alert
	{
		// Token: 0x06002B3E RID: 11070 RVA: 0x0016DB89 File Offset: 0x0016BF89
		public Alert_NeedMealSource()
		{
			this.defaultLabel = "NeedMealSource".Translate();
			this.defaultExplanation = "NeedMealSourceDesc".Translate();
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x0016DBB4 File Offset: 0x0016BFB4
		public override AlertReport GetReport()
		{
			AlertReport result;
			if (GenDate.DaysPassed < 2)
			{
				result = false;
			}
			else
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (this.NeedMealSource(maps[i]))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x0016DC24 File Offset: 0x0016C024
		private bool NeedMealSource(Map map)
		{
			bool result;
			if (!map.IsPlayerHome)
			{
				result = false;
			}
			else if (!map.mapPawns.AnyColonistSpawned)
			{
				result = false;
			}
			else
			{
				result = !map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.building.isMealSource);
			}
			return result;
		}
	}
}
