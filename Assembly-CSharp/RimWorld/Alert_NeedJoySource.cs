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
			if (GenDate.DaysPassedFloat < 6.5)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedJoySource(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		private bool NeedJoySource(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned)
			{
				return false;
			}
			if (map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.building.isJoySource))
			{
				return false;
			}
			return true;
		}
	}
}
