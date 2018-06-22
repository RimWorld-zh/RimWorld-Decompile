using System;

namespace RimWorld
{
	// Token: 0x02000254 RID: 596
	public class CompProperties_Targetable : CompProperties_UseEffect
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x0005FDCC File Offset: 0x0005E1CC
		public CompProperties_Targetable()
		{
			this.compClass = typeof(CompTargetable);
		}

		// Token: 0x040004B1 RID: 1201
		public bool psychicSensitiveTargetsOnly;

		// Token: 0x040004B2 RID: 1202
		public bool fleshCorpsesOnly;

		// Token: 0x040004B3 RID: 1203
		public bool nonDessicatedCorpsesOnly;
	}
}
