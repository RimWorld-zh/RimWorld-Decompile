using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Alert_NeedWarden : Alert
	{
		public Alert_NeedWarden()
		{
			base.defaultLabel = "NeedWarden".Translate();
			base.defaultExplanation = "NeedWardenDesc".Translate();
			base.defaultPriority = AlertPriority.High;
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
					Map map = maps[num];
					if (map.IsPlayerHome && map.mapPawns.PrisonersOfColonySpawned.Any())
					{
						bool flag = false;
						foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
						{
							if (!item.Downed && item.workSettings != null && item.workSettings.GetPriority(WorkTypeDefOf.Warden) > 0)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							result = AlertReport.CulpritIs((Thing)map.mapPawns.PrisonersOfColonySpawned.First());
							break;
						}
					}
					num++;
					continue;
				}
				result = AlertReport.Inactive;
				break;
			}
			return result;
		}
	}
}
