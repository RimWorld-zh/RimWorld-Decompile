using System;

namespace RimWorld
{
	// Token: 0x0200023F RID: 575
	public class CompProperties_Battery : CompProperties_Power
	{
		// Token: 0x06000A68 RID: 2664 RVA: 0x0005E7B3 File Offset: 0x0005CBB3
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}

		// Token: 0x0400045C RID: 1116
		public float storedEnergyMax = 1000f;

		// Token: 0x0400045D RID: 1117
		public float efficiency = 0.5f;
	}
}
