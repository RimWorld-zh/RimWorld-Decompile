using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_NeedJoySource : Alert
	{
		public Alert_NeedJoySource()
		{
			base.defaultLabel = "NeedJoySource".Translate();
			base.defaultExplanation = "NeedJoySourceDesc".Translate();
		}

		public override AlertReport GetReport()
		{
			AlertReport result;
			if (GenDate.DaysPassedFloat < 6.5)
			{
				result = false;
			}
			else
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (this.NeedJoySource(maps[i]))
						goto IL_003c;
				}
				result = false;
			}
			goto IL_0065;
			IL_003c:
			result = true;
			goto IL_0065;
			IL_0065:
			return result;
		}

		private bool NeedJoySource(Map map)
		{
			return (byte)(map.IsPlayerHome ? (map.mapPawns.AnyColonistSpawned ? ((!map.listerBuildings.allBuildingsColonist.Any((Predicate<Building>)((Building b) => b.def.building.isJoySource))) ? 1 : 0) : 0) : 0) != 0;
		}
	}
}
