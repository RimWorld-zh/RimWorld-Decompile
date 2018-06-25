using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000254 RID: 596
	public class CompProperties_SnowExpand : CompProperties
	{
		// Token: 0x040004AE RID: 1198
		public int expandInterval = 500;

		// Token: 0x040004AF RID: 1199
		public float addAmount = 0.12f;

		// Token: 0x040004B0 RID: 1200
		public float maxRadius = 55f;

		// Token: 0x06000A93 RID: 2707 RVA: 0x0005FEB0 File Offset: 0x0005E2B0
		public CompProperties_SnowExpand()
		{
			this.compClass = typeof(CompSnowExpand);
		}
	}
}
