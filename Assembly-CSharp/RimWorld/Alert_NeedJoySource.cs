using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A2 RID: 1954
	public class Alert_NeedJoySource : Alert
	{
		// Token: 0x06002B42 RID: 11074 RVA: 0x0016DCC0 File Offset: 0x0016C0C0
		public Alert_NeedJoySource()
		{
			this.defaultLabel = "NeedJoySource".Translate();
			this.defaultExplanation = "NeedJoySourceDesc".Translate();
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x0016DCEC File Offset: 0x0016C0EC
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

		// Token: 0x06002B44 RID: 11076 RVA: 0x0016DD60 File Offset: 0x0016C160
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
