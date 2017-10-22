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
			if (!map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe))
			{
				if (map.listerThings.ThingsOfDef(ThingDefOf.CrashedPsychicEmanatorShipPart).Count > 0)
				{
					return false;
				}
				if (map.mapPawns.FreeColonistsCount == 0)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(map, Mathf.RoundToInt((float)(base.def.durationDays.RandomInRange * 60000.0)), map.mapPawns.FreeColonists.RandomElement().gender);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(map);
			return true;
		}

		protected abstract void DoConditionAndLetter(Map map, int duration, Gender gender);
	}
}
