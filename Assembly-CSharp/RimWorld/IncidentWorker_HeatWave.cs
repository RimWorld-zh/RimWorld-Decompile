using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200032F RID: 815
	public class IncidentWorker_HeatWave : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06000DEB RID: 3563 RVA: 0x00076D54 File Offset: 0x00075154
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
