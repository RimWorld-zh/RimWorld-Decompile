using System;

namespace Verse
{
	// Token: 0x02000B10 RID: 2832
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x040027ED RID: 10221
		public float heatPerSecond = 0f;

		// Token: 0x040027EE RID: 10222
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x040027EF RID: 10223
		public float heatPushMinTemperature = -99999f;

		// Token: 0x06003EA2 RID: 16034 RVA: 0x0020FA08 File Offset: 0x0020DE08
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}
	}
}
