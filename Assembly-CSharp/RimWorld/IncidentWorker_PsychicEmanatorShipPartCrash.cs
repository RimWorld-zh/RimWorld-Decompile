using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200033B RID: 827
	internal class IncidentWorker_PsychicEmanatorShipPartCrash : IncidentWorker_ShipPartCrash
	{
		// Token: 0x06000E1A RID: 3610 RVA: 0x000781E0 File Offset: 0x000765E0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && base.CanFireNowSub(parms);
		}
	}
}
