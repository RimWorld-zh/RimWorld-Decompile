using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A3 RID: 1955
	public class Alert_NeedMealSource : Alert
	{
		// Token: 0x06002B3F RID: 11071 RVA: 0x0016D7CD File Offset: 0x0016BBCD
		public Alert_NeedMealSource()
		{
			this.defaultLabel = "NeedMealSource".Translate();
			this.defaultExplanation = "NeedMealSourceDesc".Translate();
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x0016D7F8 File Offset: 0x0016BBF8
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

		// Token: 0x06002B41 RID: 11073 RVA: 0x0016D868 File Offset: 0x0016BC68
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
