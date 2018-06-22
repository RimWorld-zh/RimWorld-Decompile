using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000336 RID: 822
	public abstract class IncidentWorker_PsychicEmanation : IncidentWorker
	{
		// Token: 0x06000E0F RID: 3599 RVA: 0x00077E88 File Offset: 0x00076288
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe) && map.listerThings.ThingsOfDef(ThingDefOf.CrashedPsychicEmanatorShipPart).Count <= 0 && map.mapPawns.FreeColonistsCount != 0;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00077F14 File Offset: 0x00076314
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(map, Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f), map.mapPawns.FreeColonists.RandomElement<Pawn>().gender);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(map);
			return true;
		}

		// Token: 0x06000E11 RID: 3601
		protected abstract void DoConditionAndLetter(Map map, int duration, Gender gender);
	}
}
