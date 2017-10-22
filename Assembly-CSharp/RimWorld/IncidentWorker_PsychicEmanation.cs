using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public abstract class IncidentWorker_PsychicEmanation : IncidentWorker
	{
		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			return (byte)((!map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe)) ? ((map.listerThings.ThingsOfDef(ThingDefOf.CrashedPsychicEmanatorShipPart).Count <= 0) ? ((map.mapPawns.FreeColonistsCount != 0) ? 1 : 0) : 0) : 0) != 0;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(map, Mathf.RoundToInt((float)(base.def.durationDays.RandomInRange * 60000.0)), map.mapPawns.FreeColonists.RandomElement().gender);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(map);
			return true;
		}

		protected abstract void DoConditionAndLetter(Map map, int duration, Gender gender);
	}
}
