using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000641 RID: 1601
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002125 RID: 8485 RVA: 0x00119E9C File Offset: 0x0011829C
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x00119EB8 File Offset: 0x001182B8
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
