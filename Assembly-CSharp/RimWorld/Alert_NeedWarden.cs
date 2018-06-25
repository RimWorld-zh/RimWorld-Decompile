using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000796 RID: 1942
	public class Alert_NeedWarden : Alert
	{
		// Token: 0x06002B09 RID: 11017 RVA: 0x0016BC31 File Offset: 0x0016A031
		public Alert_NeedWarden()
		{
			this.defaultLabel = "NeedWarden".Translate();
			this.defaultExplanation = "NeedWardenDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x0016BC64 File Offset: 0x0016A064
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
