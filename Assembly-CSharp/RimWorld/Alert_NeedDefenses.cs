using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Alert_NeedDefenses : Alert
	{
		[CompilerGenerated]
		private static Predicate<Building> <>f__am$cache0;

		public Alert_NeedDefenses()
		{
			this.defaultLabel = "NeedDefenses".Translate();
			this.defaultExplanation = "NeedDefensesDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		public override AlertReport GetReport()
		{
			AlertReport result;
			if (GenDate.DaysPassed < 2 || GenDate.DaysPassed > 5)
			{
				result = false;
			}
			else
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (this.NeedDefenses(maps[i]))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		private bool NeedDefenses(Map map)
		{
			bool result;
			if (!map.IsPlayerHome)
			{
				result = false;
			}
			else if (!map.mapPawns.AnyColonistSpawned && !map.listerBuildings.allBuildingsColonist.Any<Building>())
			{
				result = false;
			}
			else
			{
				result = !map.listerBuildings.allBuildingsColonist.Any((Building b) => (b.def.building != null && (b.def.building.IsTurret || b.def.building.isTrap)) || b.def == ThingDefOf.Sandbags);
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <NeedDefenses>m__0(Building b)
		{
			return (b.def.building != null && (b.def.building.IsTurret || b.def.building.isTrap)) || b.def == ThingDefOf.Sandbags;
		}
	}
}
