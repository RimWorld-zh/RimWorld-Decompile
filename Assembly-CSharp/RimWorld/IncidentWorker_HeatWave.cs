using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200032D RID: 813
	public class IncidentWorker_HeatWave : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06000DE8 RID: 3560 RVA: 0x00076B48 File Offset: 0x00074F48
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
				result = (map.mapTemperature.SeasonalTemp >= 20f);
			}
			return result;
		}
	}
}
