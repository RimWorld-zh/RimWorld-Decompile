using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200063D RID: 1597
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x0600211D RID: 8477 RVA: 0x00119F9C File Offset: 0x0011839C
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x00119FB8 File Offset: 0x001183B8
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
