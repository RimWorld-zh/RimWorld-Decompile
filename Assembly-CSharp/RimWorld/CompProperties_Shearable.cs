using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000253 RID: 595
	public class CompProperties_Shearable : CompProperties
	{
		// Token: 0x040004AD RID: 1197
		public int shearIntervalDays;

		// Token: 0x040004AE RID: 1198
		public int woolAmount = 1;

		// Token: 0x040004AF RID: 1199
		public ThingDef woolDef;

		// Token: 0x06000A91 RID: 2705 RVA: 0x0005FE8C File Offset: 0x0005E28C
		public CompProperties_Shearable()
		{
			this.compClass = typeof(CompShearable);
		}
	}
}
