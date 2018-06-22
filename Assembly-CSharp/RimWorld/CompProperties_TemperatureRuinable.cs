using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073C RID: 1852
	public class CompProperties_TemperatureRuinable : CompProperties
	{
		// Token: 0x060028F5 RID: 10485 RVA: 0x0015D9F1 File Offset: 0x0015BDF1
		public CompProperties_TemperatureRuinable()
		{
			this.compClass = typeof(CompTemperatureRuinable);
		}

		// Token: 0x04001668 RID: 5736
		public float minSafeTemperature = 0f;

		// Token: 0x04001669 RID: 5737
		public float maxSafeTemperature = 100f;

		// Token: 0x0400166A RID: 5738
		public float progressPerDegreePerTick = 1E-05f;
	}
}
