using System;

namespace Verse
{
	// Token: 0x02000B12 RID: 2834
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x06003EA2 RID: 16034 RVA: 0x0020F5A0 File Offset: 0x0020D9A0
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}

		// Token: 0x040027F0 RID: 10224
		public float heatPerSecond = 0f;

		// Token: 0x040027F1 RID: 10225
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x040027F2 RID: 10226
		public float heatPushMinTemperature = -99999f;
	}
}
