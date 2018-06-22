using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079F RID: 1951
	public class Alert_NeedMealSource : Alert
	{
		// Token: 0x06002B3A RID: 11066 RVA: 0x0016DA39 File Offset: 0x0016BE39
		public Alert_NeedMealSource()
		{
			this.defaultLabel = "NeedMealSource".Translate();
			this.defaultExplanation = "NeedMealSourceDesc".Translate();
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x0016DA64 File Offset: 0x0016BE64
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

		// Token: 0x06002B3C RID: 11068 RVA: 0x0016DAD4 File Offset: 0x0016BED4
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
