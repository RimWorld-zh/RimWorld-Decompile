using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200063F RID: 1599
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002121 RID: 8481 RVA: 0x0011A0EC File Offset: 0x001184EC
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x0011A108 File Offset: 0x00118508
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
