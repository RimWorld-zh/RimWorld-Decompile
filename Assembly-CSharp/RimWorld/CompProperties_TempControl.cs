using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000257 RID: 599
	public class CompProperties_TempControl : CompProperties
	{
		// Token: 0x040004B6 RID: 1206
		public float energyPerSecond = 12f;

		// Token: 0x040004B7 RID: 1207
		public float defaultTargetTemperature = 21f;

		// Token: 0x040004B8 RID: 1208
		public float minTargetTemperature = -50f;

		// Token: 0x040004B9 RID: 1209
		public float maxTargetTemperature = 50f;

		// Token: 0x040004BA RID: 1210
		public float lowPowerConsumptionFactor = 0.1f;

		// Token: 0x06000A95 RID: 2709 RVA: 0x0005FF34 File Offset: 0x0005E334
		public CompProperties_TempControl()
		{
			this.compClass = typeof(CompTempControl);
		}
	}
}
