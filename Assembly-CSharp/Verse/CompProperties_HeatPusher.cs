using System;

namespace Verse
{
	// Token: 0x02000B0E RID: 2830
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x040027EC RID: 10220
		public float heatPerSecond = 0f;

		// Token: 0x040027ED RID: 10221
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x040027EE RID: 10222
		public float heatPushMinTemperature = -99999f;

		// Token: 0x06003E9E RID: 16030 RVA: 0x0020F8DC File Offset: 0x0020DCDC
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}
	}
}
