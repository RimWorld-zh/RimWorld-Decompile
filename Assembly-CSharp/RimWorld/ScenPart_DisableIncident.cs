using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200063F RID: 1599
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x0011A354 File Offset: 0x00118754
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x0011A370 File Offset: 0x00118770
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.TraderCaravanArrival;
			yield return IncidentDefOf.OrbitalTraderArrival;
			yield return IncidentDefOf.WandererJoin;
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}
	}
}
