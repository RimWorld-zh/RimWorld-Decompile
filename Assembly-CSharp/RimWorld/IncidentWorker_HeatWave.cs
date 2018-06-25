using System;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_HeatWave : IncidentWorker_MakeGameCondition
	{
		public IncidentWorker_HeatWave()
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
				result = (map.mapTemperature.SeasonalTemp >= 20f);
			}
			return result;
		}
	}
}
