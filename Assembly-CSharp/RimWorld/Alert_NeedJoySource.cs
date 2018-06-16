using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A4 RID: 1956
	public class Alert_NeedJoySource : Alert
	{
		// Token: 0x06002B43 RID: 11075 RVA: 0x0016D904 File Offset: 0x0016BD04
		public Alert_NeedJoySource()
		{
			this.defaultLabel = "NeedJoySource".Translate();
			this.defaultExplanation = "NeedJoySourceDesc".Translate();
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x0016D930 File Offset: 0x0016BD30
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

		// Token: 0x06002B45 RID: 11077 RVA: 0x0016D9A4 File Offset: 0x0016BDA4
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
