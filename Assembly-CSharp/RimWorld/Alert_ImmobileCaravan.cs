using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000790 RID: 1936
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		// Token: 0x06002AF0 RID: 10992 RVA: 0x0016B314 File Offset: 0x00169714
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x0016B340 File Offset: 0x00169740
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

		// Token: 0x06002AF2 RID: 10994 RVA: 0x0016B364 File Offset: 0x00169764
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}
	}
}
