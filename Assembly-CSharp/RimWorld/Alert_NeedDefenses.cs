using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000793 RID: 1939
	public class Alert_NeedDefenses : Alert
	{
		// Token: 0x06002AFD RID: 11005 RVA: 0x0016B715 File Offset: 0x00169B15
		public Alert_NeedDefenses()
		{
			this.defaultLabel = "NeedDefenses".Translate();
			this.defaultExplanation = "NeedDefensesDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x0016B748 File Offset: 0x00169B48
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

		// Token: 0x06002AFF RID: 11007 RVA: 0x0016B7C4 File Offset: 0x00169BC4
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
	}
}
