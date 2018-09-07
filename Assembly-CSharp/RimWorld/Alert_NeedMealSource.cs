using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Alert_NeedMealSource : Alert
	{
		[CompilerGenerated]
		private static Predicate<Building> <>f__am$cache0;

		public Alert_NeedMealSource()
		{
			this.defaultLabel = "NeedMealSource".Translate();
			this.defaultExplanation = "NeedMealSourceDesc".Translate();
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
			return !map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.building.isMealSource);
		}

		[CompilerGenerated]
		private static bool <NeedMealSource>m__0(Building b)
		{
			return b.def.building.isMealSource;
		}
	}
}
