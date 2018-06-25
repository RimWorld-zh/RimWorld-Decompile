using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200033B RID: 827
	internal class IncidentWorker_PsychicEmanatorShipPartCrash : IncidentWorker_ShipPartCrash
	{
		// Token: 0x06000E1B RID: 3611 RVA: 0x000781D8 File Offset: 0x000765D8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && base.CanFireNowSub(parms);
		}
	}
}
