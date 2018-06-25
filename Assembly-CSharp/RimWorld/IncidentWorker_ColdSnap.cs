using System;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		public IncidentWorker_ColdSnap()
		{
		}

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
