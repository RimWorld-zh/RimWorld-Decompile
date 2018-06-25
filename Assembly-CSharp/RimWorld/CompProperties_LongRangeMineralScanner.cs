using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024B RID: 587
	public class CompProperties_LongRangeMineralScanner : CompProperties
	{
		// Token: 0x04000494 RID: 1172
		public float radius = 30f;

		// Token: 0x04000495 RID: 1173
		public float mtbDays = 30f;

		// Token: 0x06000A80 RID: 2688 RVA: 0x0005F484 File Offset: 0x0005D884
		public CompProperties_LongRangeMineralScanner()
		{
			this.compClass = typeof(CompLongRangeMineralScanner);
		}
	}
}
