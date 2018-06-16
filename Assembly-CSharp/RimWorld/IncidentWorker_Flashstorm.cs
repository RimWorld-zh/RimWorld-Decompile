using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200032C RID: 812
	public class IncidentWorker_Flashstorm : IncidentWorker
	{
		// Token: 0x06000DE5 RID: 3557 RVA: 0x00076A64 File Offset: 0x00074E64
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Flashstorm);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00076A98 File Offset: 0x00074E98
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition_Flashstorm gameCondition_Flashstorm = (GameCondition_Flashstorm)GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm, duration, 0);
			map.gameConditionManager.RegisterCondition(gameCondition_Flashstorm);
			base.SendStandardLetter(new TargetInfo(gameCondition_Flashstorm.centerLocation.ToIntVec3, map, false), null, new string[0]);
			if (map.weatherManager.curWeather.rainRate > 0.1f)
			{
				map.weatherDecider.StartNextWeather();
			}
			return true;
		}
	}
}
