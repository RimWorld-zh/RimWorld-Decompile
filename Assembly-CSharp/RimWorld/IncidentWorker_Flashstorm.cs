using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200032E RID: 814
	public class IncidentWorker_Flashstorm : IncidentWorker
	{
		// Token: 0x06000DE9 RID: 3561 RVA: 0x00076C68 File Offset: 0x00075068
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Flashstorm);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00076C9C File Offset: 0x0007509C
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
