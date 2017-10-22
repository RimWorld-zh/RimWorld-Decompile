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
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
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
						return AlertReport.CulpritIs((Thing)map.mapPawns.PrisonersOfColonySpawned.First());
					}
				}
			}
			return AlertReport.Inactive;
		}
	}
}
