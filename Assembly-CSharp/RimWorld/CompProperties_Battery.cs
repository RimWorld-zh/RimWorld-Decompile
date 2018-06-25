using System;

namespace RimWorld
{
	// Token: 0x02000241 RID: 577
	public class CompProperties_Battery : CompProperties_Power
	{
		// Token: 0x0400045E RID: 1118
		public float storedEnergyMax = 1000f;

		// Token: 0x0400045F RID: 1119
		public float efficiency = 0.5f;

		// Token: 0x06000A6B RID: 2667 RVA: 0x0005E8FF File Offset: 0x0005CCFF
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}
	}
}
