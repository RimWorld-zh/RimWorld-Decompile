using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000641 RID: 1601
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002123 RID: 8483 RVA: 0x00119E24 File Offset: 0x00118224
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00119E40 File Offset: 0x00118240
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
