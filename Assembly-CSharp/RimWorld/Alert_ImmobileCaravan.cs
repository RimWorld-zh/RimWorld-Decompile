using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078E RID: 1934
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		// Token: 0x06002AED RID: 10989 RVA: 0x0016AF60 File Offset: 0x00169360
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002AEE RID: 10990 RVA: 0x0016AF8C File Offset: 0x0016938C
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

		// Token: 0x06002AEF RID: 10991 RVA: 0x0016AFB0 File Offset: 0x001693B0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}
	}
}
