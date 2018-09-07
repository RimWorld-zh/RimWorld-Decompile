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
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return map.mapTemperature.SeasonalTemp >= 20f;
		}
	}
}
