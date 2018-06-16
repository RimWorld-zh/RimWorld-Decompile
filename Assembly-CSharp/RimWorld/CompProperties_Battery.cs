using System;

namespace RimWorld
{
	// Token: 0x0200023F RID: 575
	public class CompProperties_Battery : CompProperties_Power
	{
		// Token: 0x06000A6A RID: 2666 RVA: 0x0005E757 File Offset: 0x0005CB57
		public CompProperties_Battery()
		{
			this.compClass = typeof(CompPowerBattery);
		}

		// Token: 0x0400045E RID: 1118
		public float storedEnergyMax = 1000f;

		// Token: 0x0400045F RID: 1119
		public float efficiency = 0.5f;
	}
}
