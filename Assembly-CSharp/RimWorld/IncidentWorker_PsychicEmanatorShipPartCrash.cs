using Verse;

namespace RimWorld
{
	internal class IncidentWorker_PsychicEmanatorShipPartCrash : IncidentWorker_ShipPartCrash
	{
		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			if (map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone))
			{
				return false;
			}
			return base.CanFireNowSub(target);
		}
	}
}
