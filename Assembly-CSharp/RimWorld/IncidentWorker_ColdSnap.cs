using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000330 RID: 816
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		// Token: 0x06000DED RID: 3565 RVA: 0x00076DA8 File Offset: 0x000751A8
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
