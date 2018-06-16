using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000249 RID: 585
	public class CompProperties_LongRangeMineralScanner : CompProperties
	{
		// Token: 0x06000A7F RID: 2687 RVA: 0x0005F2DC File Offset: 0x0005D6DC
		public CompProperties_LongRangeMineralScanner()
		{
			this.compClass = typeof(CompLongRangeMineralScanner);
		}

		// Token: 0x04000494 RID: 1172
		public float radius = 30f;

		// Token: 0x04000495 RID: 1173
		public float mtbDays = 30f;
	}
}
