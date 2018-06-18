using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000792 RID: 1938
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		// Token: 0x06002AF4 RID: 10996 RVA: 0x0016AD88 File Offset: 0x00169188
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x0016ADB4 File Offset: 0x001691B4
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

		// Token: 0x06002AF6 RID: 10998 RVA: 0x0016ADD8 File Offset: 0x001691D8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}
	}
}
