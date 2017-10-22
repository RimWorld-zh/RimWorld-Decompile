using Verse;

namespace RimWorld
{
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
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
				result = (map.mapTemperature.SeasonalTemp > 0.0 && map.mapTemperature.SeasonalTemp < 15.0);
			}
			return result;
		}
	}
}
