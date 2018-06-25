using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A1 RID: 1953
	public class Alert_NeedMealSource : Alert
	{
		// Token: 0x06002B3D RID: 11069 RVA: 0x0016DDED File Offset: 0x0016C1ED
		public Alert_NeedMealSource()
		{
			this.defaultLabel = "NeedMealSource".Translate();
			this.defaultExplanation = "NeedMealSourceDesc".Translate();
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x0016DE18 File Offset: 0x0016C218
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

		// Token: 0x06002B3F RID: 11071 RVA: 0x0016DE88 File Offset: 0x0016C288
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
