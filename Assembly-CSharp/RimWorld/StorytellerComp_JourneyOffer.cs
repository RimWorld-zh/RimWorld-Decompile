using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200036F RID: 879
	public class StorytellerComp_JourneyOffer : StorytellerComp
	{
		// Token: 0x04000952 RID: 2386
		private const int StartOnDay = 14;

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x00080F64 File Offset: 0x0007F364
		private int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00080F8C File Offset: 0x0007F38C
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
	}
}
