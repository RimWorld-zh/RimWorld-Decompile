using System;

namespace Verse
{
	// Token: 0x02000B11 RID: 2833
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x040027F4 RID: 10228
		public float heatPerSecond = 0f;

		// Token: 0x040027F5 RID: 10229
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x040027F6 RID: 10230
		public float heatPushMinTemperature = -99999f;

		// Token: 0x06003EA2 RID: 16034 RVA: 0x0020FCE8 File Offset: 0x0020E0E8
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}
	}
}
