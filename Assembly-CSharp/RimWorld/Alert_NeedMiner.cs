using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Alert_NeedMiner : Alert
	{
		public Alert_NeedMiner()
		{
			base.defaultLabel = "NeedMiner".Translate();
			base.defaultExplanation = "NeedMinerDesc".Translate();
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
					if (map.IsPlayerHome)
					{
						Designation designation = (from d in map.designationManager.allDesignations
						where d.def == DesignationDefOf.Mine
						select d).FirstOrDefault();
						if (designation != null)
						{
							bool flag = false;
							foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
							{
								if (!item.Downed && item.workSettings != null && item.workSettings.GetPriority(WorkTypeDefOf.Mining) > 0)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								result = AlertReport.CulpritIs(designation.target.Thing);
								break;
							}
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
