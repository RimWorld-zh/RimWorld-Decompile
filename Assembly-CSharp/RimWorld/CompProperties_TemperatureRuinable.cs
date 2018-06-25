using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073E RID: 1854
	public class CompProperties_TemperatureRuinable : CompProperties
	{
		// Token: 0x04001668 RID: 5736
		public float minSafeTemperature = 0f;

		// Token: 0x04001669 RID: 5737
		public float maxSafeTemperature = 100f;

		// Token: 0x0400166A RID: 5738
		public float progressPerDegreePerTick = 1E-05f;

		// Token: 0x060028F9 RID: 10489 RVA: 0x0015DB41 File Offset: 0x0015BF41
		public CompProperties_TemperatureRuinable()
		{
			this.compClass = typeof(CompTemperatureRuinable);
		}
	}
}
