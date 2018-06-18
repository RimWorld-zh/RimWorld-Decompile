using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000798 RID: 1944
	public class Alert_NeedWarden : Alert
	{
		// Token: 0x06002B0C RID: 11020 RVA: 0x0016B909 File Offset: 0x00169D09
		public Alert_NeedWarden()
		{
			this.defaultLabel = "NeedWarden".Translate();
			this.defaultExplanation = "NeedWardenDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x0016B93C File Offset: 0x00169D3C
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
