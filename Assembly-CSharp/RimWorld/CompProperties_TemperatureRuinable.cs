using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000740 RID: 1856
	public class CompProperties_TemperatureRuinable : CompProperties
	{
		// Token: 0x060028FA RID: 10490 RVA: 0x0015D785 File Offset: 0x0015BB85
		public CompProperties_TemperatureRuinable()
		{
			this.compClass = typeof(CompTemperatureRuinable);
		}

		// Token: 0x0400166A RID: 5738
		public float minSafeTemperature = 0f;

		// Token: 0x0400166B RID: 5739
		public float maxSafeTemperature = 100f;

		// Token: 0x0400166C RID: 5740
		public float progressPerDegreePerTick = 1E-05f;
	}
}
