using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A3 RID: 1955
	public class Alert_NeedBatteries : Alert
	{
		// Token: 0x06002B45 RID: 11077 RVA: 0x0016E066 File Offset: 0x0016C466
		public Alert_NeedBatteries()
		{
			this.defaultLabel = "NeedBatteries".Translate();
			this.defaultExplanation = "NeedBatteriesDesc".Translate();
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x0016E090 File Offset: 0x0016C490
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedBatteries(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x0016E0E8 File Offset: 0x0016C4E8
		private bool NeedBatteries(Map map)
		{
			bool result;
			if (!map.IsPlayerHome)
			{
				result = false;
			}
			else
			{
				result = (!map.listerBuildings.ColonistsHaveBuilding((Thing building) => building is Building_Battery) && (map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.SolarGenerator) || map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.WindTurbine)) && !map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.GeothermalGenerator) && !map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.WatermillGenerator));
			}
			return result;
		}
	}
}
