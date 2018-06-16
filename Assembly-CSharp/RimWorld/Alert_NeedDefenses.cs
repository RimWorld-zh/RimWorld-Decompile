using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000795 RID: 1941
	public class Alert_NeedDefenses : Alert
	{
		// Token: 0x06002AFF RID: 11007 RVA: 0x0016B0F5 File Offset: 0x001694F5
		public Alert_NeedDefenses()
		{
			this.defaultLabel = "NeedDefenses".Translate();
			this.defaultExplanation = "NeedDefensesDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x0016B128 File Offset: 0x00169528
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

		// Token: 0x06002B01 RID: 11009 RVA: 0x0016B1A4 File Offset: 0x001695A4
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
