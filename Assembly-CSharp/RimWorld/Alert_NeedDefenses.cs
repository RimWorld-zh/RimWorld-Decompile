using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000791 RID: 1937
	public class Alert_NeedDefenses : Alert
	{
		// Token: 0x06002AFA RID: 11002 RVA: 0x0016B361 File Offset: 0x00169761
		public Alert_NeedDefenses()
		{
			this.defaultLabel = "NeedDefenses".Translate();
			this.defaultExplanation = "NeedDefensesDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x0016B394 File Offset: 0x00169794
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

		// Token: 0x06002AFC RID: 11004 RVA: 0x0016B410 File Offset: 0x00169810
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
