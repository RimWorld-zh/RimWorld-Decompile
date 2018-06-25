using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024B RID: 587
	public class CompProperties_LongRangeMineralScanner : CompProperties
	{
		// Token: 0x04000492 RID: 1170
		public float radius = 30f;

		// Token: 0x04000493 RID: 1171
		public float mtbDays = 30f;

		// Token: 0x06000A81 RID: 2689 RVA: 0x0005F488 File Offset: 0x0005D888
		public CompProperties_LongRangeMineralScanner()
		{
			this.compClass = typeof(CompLongRangeMineralScanner);
		}
	}
}
