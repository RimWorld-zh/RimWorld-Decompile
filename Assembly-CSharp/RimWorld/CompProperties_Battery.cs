using System;

namespace RimWorld
{
	// Token: 0x02000241 RID: 577
	public class CompProperties_Battery : CompProperties_Power
	{
		// Token: 0x0400045C RID: 1116
		public float storedEnergyMax = 1000f;

		// Token: 0x0400045D RID: 1117
		public float efficiency = 0.5f;

		// Token: 0x06000A6C RID: 2668 RVA: 0x0005E903 File Offset: 0x0005CD03
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}
	}
}
