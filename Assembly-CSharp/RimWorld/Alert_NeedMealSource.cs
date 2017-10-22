using System;
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
						goto IL_0038;
				}
				result = false;
			}
			goto IL_0061;
			IL_0038:
			result = true;
			goto IL_0061;
			IL_0061:
			return result;
		}

		private bool NeedMealSource(Map map)
		{
			return (byte)(map.IsPlayerHome ? (map.mapPawns.AnyColonistSpawned ? ((!map.listerBuildings.allBuildingsColonist.Any((Predicate<Building>)((Building b) => b.def.building.isMealSource))) ? 1 : 0) : 0) : 0) != 0;
		}
	}
}
