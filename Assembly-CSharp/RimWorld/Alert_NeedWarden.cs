using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000794 RID: 1940
	public class Alert_NeedWarden : Alert
	{
		// Token: 0x06002B05 RID: 11013 RVA: 0x0016BAE1 File Offset: 0x00169EE1
		public Alert_NeedWarden()
		{
			this.defaultLabel = "NeedWarden".Translate();
			this.defaultExplanation = "NeedWardenDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x0016BB14 File Offset: 0x00169F14
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					if (map.mapPawns.PrisonersOfColonySpawned.Any<Pawn>())
					{
						bool flag = false;
						foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
						{
							if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Warden) > 0)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return AlertReport.CulpritIs(map.mapPawns.PrisonersOfColonySpawned.First<Pawn>());
						}
					}
				}
			}
			return false;
		}
	}
}
