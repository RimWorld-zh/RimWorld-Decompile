using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000338 RID: 824
	public abstract class IncidentWorker_PsychicEmanation : IncidentWorker
	{
		// Token: 0x06000E13 RID: 3603 RVA: 0x00077FD8 File Offset: 0x000763D8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe) && map.listerThings.ThingsOfDef(ThingDefOf.CrashedPsychicEmanatorShipPart).Count <= 0 && map.mapPawns.FreeColonistsCount != 0;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00078064 File Offset: 0x00076464
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(map, Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f), map.mapPawns.FreeColonists.RandomElement<Pawn>().gender);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(map);
			return true;
		}

		// Token: 0x06000E15 RID: 3605
		protected abstract void DoConditionAndLetter(Map map, int duration, Gender gender);
	}
}
