using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200036D RID: 877
	public class StorytellerComp_JourneyOffer : StorytellerComp
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x00080C28 File Offset: 0x0007F028
		private int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00080C50 File Offset: 0x0007F050
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (this.IntervalsPassed == 840)
			{
				IncidentDef inc = IncidentDefOf.Quest_JourneyOffer;
				if (inc.TargetAllowed(target))
				{
					FiringIncident fi = new FiringIncident(inc, this, this.GenerateParms(inc.category, target));
					yield return fi;
				}
			}
			yield break;
		}

		// Token: 0x04000950 RID: 2384
		private const int StartOnDay = 14;
	}
}
