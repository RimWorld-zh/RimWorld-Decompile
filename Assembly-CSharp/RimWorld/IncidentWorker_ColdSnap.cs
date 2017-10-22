using Verse;

namespace RimWorld
{
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (!base.CanFireNowSub(target))
			{
				return false;
			}
			Map map = (Map)target;
			return map.mapTemperature.SeasonalTemp > 0.0 && map.mapTemperature.SeasonalTemp < 15.0;
		}
	}
}
