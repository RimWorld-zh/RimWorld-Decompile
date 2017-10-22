using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_NeedDefenses : Alert
	{
		public Alert_NeedDefenses()
		{
			base.defaultLabel = "NeedDefenses".Translate();
			base.defaultExplanation = "NeedDefensesDesc".Translate();
			base.defaultPriority = AlertPriority.High;
		}

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
						goto IL_0043;
				}
				result = false;
			}
			goto IL_006c;
			IL_0043:
			result = true;
			goto IL_006c;
			IL_006c:
			return result;
		}

		private bool NeedDefenses(Map map)
		{
			return (byte)(map.IsPlayerHome ? ((map.mapPawns.AnyColonistSpawned || map.listerBuildings.allBuildingsColonist.Any()) ? ((!map.listerBuildings.allBuildingsColonist.Any((Predicate<Building>)((Building b) => (b.def.building != null && (b.def.building.IsTurret || b.def.building.isTrap)) || b.def == ThingDefOf.Sandbags))) ? 1 : 0) : 0) : 0) != 0;
		}
	}
}
