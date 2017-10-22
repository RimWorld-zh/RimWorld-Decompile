using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_NeedBatteries : Alert
	{
		public Alert_NeedBatteries()
		{
			base.defaultLabel = "NeedBatteries".Translate();
			base.defaultExplanation = "NeedBatteriesDesc".Translate();
		}

		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			AlertReport result;
			while (true)
			{
				if (num < maps.Count)
				{
					if (this.NeedBatteries(maps[num]))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		private bool NeedBatteries(Map map)
		{
			return (byte)(map.IsPlayerHome ? ((!map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.Battery)) ? ((map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.SolarGenerator) || map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.WindTurbine)) ? ((!map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.GeothermalGenerator)) ? 1 : 0) : 0) : 0) : 0) != 0;
		}
	}
}
