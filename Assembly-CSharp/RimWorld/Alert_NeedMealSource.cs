using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_NeedMealSource : Alert
	{
		public Alert_NeedMealSource()
		{
			base.defaultLabel = "NeedMealSource".Translate();
			base.defaultExplanation = "NeedMealSourceDesc".Translate();
		}

		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 2)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedMealSource(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		private bool NeedMealSource(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned)
			{
				return false;
			}
			if (map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.building.isMealSource))
			{
				return false;
			}
			return true;
		}
	}
}
