using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000251 RID: 593
	public class CompProperties_Shearable : CompProperties
	{
		// Token: 0x06000A90 RID: 2704 RVA: 0x0005FCE4 File Offset: 0x0005E0E4
		public CompProperties_Shearable()
		{
			this.compClass = typeof(CompShearable);
		}

		// Token: 0x040004AD RID: 1197
		public int shearIntervalDays;

		// Token: 0x040004AE RID: 1198
		public int woolAmount = 1;

		// Token: 0x040004AF RID: 1199
		public ThingDef woolDef;
	}
}
