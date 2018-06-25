using System;

namespace RimWorld
{
	// Token: 0x02000256 RID: 598
	public class CompProperties_Targetable : CompProperties_UseEffect
	{
		// Token: 0x040004B3 RID: 1203
		public bool psychicSensitiveTargetsOnly;

		// Token: 0x040004B4 RID: 1204
		public bool fleshCorpsesOnly;

		// Token: 0x040004B5 RID: 1205
		public bool nonDessicatedCorpsesOnly;

		// Token: 0x06000A94 RID: 2708 RVA: 0x0005FF18 File Offset: 0x0005E318
		public CompProperties_Targetable()
		{
			this.compClass = typeof(CompTargetable);
		}
	}
}
