using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A1 RID: 1953
	public class Alert_NeedBatteries : Alert
	{
		// Token: 0x06002B42 RID: 11074 RVA: 0x0016DCB2 File Offset: 0x0016C0B2
		public Alert_NeedBatteries()
		{
			this.defaultLabel = "NeedBatteries".Translate();
			this.defaultExplanation = "NeedBatteriesDesc".Translate();
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x0016DCDC File Offset: 0x0016C0DC
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

		// Token: 0x06002B44 RID: 11076 RVA: 0x0016DD34 File Offset: 0x0016C134
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
