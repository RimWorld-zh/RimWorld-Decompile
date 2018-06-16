using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000792 RID: 1938
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		// Token: 0x06002AF2 RID: 10994 RVA: 0x0016ACF4 File Offset: 0x001690F4
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x0016AD20 File Offset: 0x00169120
		private IEnumerable<Caravan> ImmobileCaravans
		{
			get
			{
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < caravans.Count; i++)
				{
					if (caravans[i].IsPlayerControlled && caravans[i].ImmobilizedByMass)
					{
						yield return caravans[i];
					}
				}
				yield break;
			}
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x0016AD44 File Offset: 0x00169144
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}
	}
}
