using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000251 RID: 593
	public class CompProperties_Shearable : CompProperties
	{
		// Token: 0x040004AB RID: 1195
		public int shearIntervalDays;

		// Token: 0x040004AC RID: 1196
		public int woolAmount = 1;

		// Token: 0x040004AD RID: 1197
		public ThingDef woolDef;

		// Token: 0x06000A8E RID: 2702 RVA: 0x0005FD40 File Offset: 0x0005E140
		public CompProperties_Shearable()
		{
			this.compClass = typeof(CompShearable);
		}
	}
}
