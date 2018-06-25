using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200034F RID: 847
	public class IncidentWorker_Aurora : IncidentWorker_MakeGameCondition
	{
		// Token: 0x040008FC RID: 2300
		private const int EnsureMinDurationTicks = 5000;

		// Token: 0x06000E99 RID: 3737 RVA: 0x0007BB2C File Offset: 0x00079F2C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome && !this.AuroraWillEndSoon(maps[i]))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0007BBA0 File Offset: 0x00079FA0
		private bool AuroraWillEndSoon(Map map)
		{
			return GenCelestial.CurCelestialSunGlow(map) > 0.5f || GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs + 5000) > 0.5f;
		}
	}
}
