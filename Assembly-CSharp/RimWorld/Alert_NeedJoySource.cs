using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class Alert_NeedJoySource : Alert
	{
		[CompilerGenerated]
		private static Predicate<Building> <>f__am$cache0;

		public Alert_NeedJoySource()
		{
			this.defaultLabel = "NeedJoySource".Translate();
			this.defaultExplanation = "NeedJoySourceDesc".Translate();
		}

		public override AlertReport GetReport()
		{
			AlertReport result;
			if (GenDate.DaysPassedFloat < 6.5f)
			{
				result = false;
			}
			else
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (this.NeedJoySource(maps[i]))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		private bool NeedJoySource(Map map)
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
				result = !map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.building.joyKind != null);
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <NeedJoySource>m__0(Building b)
		{
			return b.def.building.joyKind != null;
		}
	}
}
