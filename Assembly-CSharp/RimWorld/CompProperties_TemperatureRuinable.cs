using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073E RID: 1854
	public class CompProperties_TemperatureRuinable : CompProperties
	{
		// Token: 0x0400166C RID: 5740
		public float minSafeTemperature = 0f;

		// Token: 0x0400166D RID: 5741
		public float maxSafeTemperature = 100f;

		// Token: 0x0400166E RID: 5742
		public float progressPerDegreePerTick = 1E-05f;

		// Token: 0x060028F8 RID: 10488 RVA: 0x0015DDA1 File Offset: 0x0015C1A1
		public CompProperties_TemperatureRuinable()
		{
			this.compClass = typeof(CompTemperatureRuinable);
		}
	}
}
