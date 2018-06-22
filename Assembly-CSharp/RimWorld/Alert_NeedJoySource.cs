using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A0 RID: 1952
	public class Alert_NeedJoySource : Alert
	{
		// Token: 0x06002B3E RID: 11070 RVA: 0x0016DB70 File Offset: 0x0016BF70
		public Alert_NeedJoySource()
		{
			this.defaultLabel = "NeedJoySource".Translate();
			this.defaultExplanation = "NeedJoySourceDesc".Translate();
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x0016DB9C File Offset: 0x0016BF9C
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

		// Token: 0x06002B40 RID: 11072 RVA: 0x0016DC10 File Offset: 0x0016C010
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
	}
}
