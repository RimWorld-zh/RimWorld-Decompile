using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000255 RID: 597
	public class CompProperties_TempControl : CompProperties
	{
		// Token: 0x040004B4 RID: 1204
		public float energyPerSecond = 12f;

		// Token: 0x040004B5 RID: 1205
		public float defaultTargetTemperature = 21f;

		// Token: 0x040004B6 RID: 1206
		public float minTargetTemperature = -50f;

		// Token: 0x040004B7 RID: 1207
		public float maxTargetTemperature = 50f;

		// Token: 0x040004B8 RID: 1208
		public float lowPowerConsumptionFactor = 0.1f;

		// Token: 0x06000A92 RID: 2706 RVA: 0x0005FDE8 File Offset: 0x0005E1E8
		public CompProperties_TempControl()
		{
			this.compClass = typeof(CompTempControl);
		}
	}
}
