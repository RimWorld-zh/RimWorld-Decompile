using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000249 RID: 585
	public class CompProperties_LongRangeMineralScanner : CompProperties
	{
		// Token: 0x04000492 RID: 1170
		public float radius = 30f;

		// Token: 0x04000493 RID: 1171
		public float mtbDays = 30f;

		// Token: 0x06000A7D RID: 2685 RVA: 0x0005F338 File Offset: 0x0005D738
		public CompProperties_LongRangeMineralScanner()
		{
			this.compClass = typeof(CompLongRangeMineralScanner);
		}
	}
}
