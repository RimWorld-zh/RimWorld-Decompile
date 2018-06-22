using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000793 RID: 1939
	public class Alert_NeedMiner : Alert
	{
		// Token: 0x06002B02 RID: 11010 RVA: 0x0016B945 File Offset: 0x00169D45
		public Alert_NeedMiner()
		{
			this.defaultLabel = "NeedMiner".Translate();
			this.defaultExplanation = "NeedMinerDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x0016B978 File Offset: 0x00169D78
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					Designation designation = (from d in map.designationManager.allDesignations
					where d.def == DesignationDefOf.Mine
					select d).FirstOrDefault<Designation>();
					if (designation != null)
					{
						bool flag = false;
						foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
						{
							if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Mining) > 0)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return AlertReport.CulpritIs(designation.target.Thing);
						}
					}
				}
			}
			return false;
		}
	}
}
