using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000339 RID: 825
	internal class IncidentWorker_PsychicEmanatorShipPartCrash : IncidentWorker_ShipPartCrash
	{
		// Token: 0x06000E17 RID: 3607 RVA: 0x00077FD4 File Offset: 0x000763D4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && base.CanFireNowSub(parms);
		}
	}
}
