using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_Flashstorm : IncidentWorker
	{
		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Flashstorm);
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int duration = Mathf.RoundToInt((float)(base.def.durationDays.RandomInRange * 60000.0));
			GameCondition_Flashstorm gameCondition_Flashstorm = (GameCondition_Flashstorm)GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm, duration, 0);
			map.gameConditionManager.RegisterCondition(gameCondition_Flashstorm);
			base.SendStandardLetter(new TargetInfo(gameCondition_Flashstorm.centerLocation.ToIntVec3, map, false));
			if (map.weatherManager.curWeather.rainRate > 0.10000000149011612)
			{
				map.weatherDecider.StartNextWeather();
			}
			return true;
		}
	}
}
