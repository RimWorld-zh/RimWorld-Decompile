using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200032E RID: 814
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06000DEA RID: 3562 RVA: 0x00076B9C File Offset: 0x00074F9C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				result = (map.mapTemperature.SeasonalTemp > 0f && map.mapTemperature.SeasonalTemp < 15f);
			}
			return result;
		}
	}
}
