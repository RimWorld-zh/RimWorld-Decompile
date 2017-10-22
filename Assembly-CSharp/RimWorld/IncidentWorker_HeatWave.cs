using Verse;

namespace RimWorld
{
	public class IncidentWorker_HeatWave : IncidentWorker_MakeGameCondition
	{
		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			bool result;
			if (!base.CanFireNowSub(target))
			{
				result = false;
			}
			else
			{
				Map map = (Map)target;
				result = (map.mapTemperature.SeasonalTemp >= 20.0);
			}
			return result;
		}
	}
}
