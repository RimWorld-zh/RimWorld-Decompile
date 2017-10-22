using Verse;

namespace RimWorld
{
	public class IncidentWorker_Aurora : IncidentWorker_MakeGameCondition
	{
		private const int EnsureMinDurationTicks = 5000;

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
				result = ((byte)((!(GenCelestial.CurCelestialSunGlow(map) > 0.5)) ? ((!(GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs + 5000) > 0.5)) ? 1 : 0) : 0) != 0);
			}
			return result;
		}
	}
}
