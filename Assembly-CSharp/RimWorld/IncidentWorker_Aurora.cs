using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200034D RID: 845
	public class IncidentWorker_Aurora : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06000E95 RID: 3733 RVA: 0x0007B9DC File Offset: 0x00079DDC
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

		// Token: 0x06000E96 RID: 3734 RVA: 0x0007BA50 File Offset: 0x00079E50
		private bool AuroraWillEndSoon(Map map)
		{
			return GenCelestial.CurCelestialSunGlow(map) > 0.5f || GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs + 5000) > 0.5f;
		}

		// Token: 0x040008FC RID: 2300
		private const int EnsureMinDurationTicks = 5000;
	}
}
