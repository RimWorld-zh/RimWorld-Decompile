using System;

namespace RimWorld
{
	// Token: 0x02000254 RID: 596
	public class CompProperties_Targetable : CompProperties_UseEffect
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x0005FD70 File Offset: 0x0005E170
		public CompProperties_Targetable()
		{
			this.compClass = typeof(CompTargetable);
		}

		// Token: 0x040004B3 RID: 1203
		public bool psychicSensitiveTargetsOnly;

		// Token: 0x040004B4 RID: 1204
		public bool fleshCorpsesOnly;

		// Token: 0x040004B5 RID: 1205
		public bool nonDessicatedCorpsesOnly;
	}
}
