using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000252 RID: 594
	public class CompProperties_SnowExpand : CompProperties
	{
		// Token: 0x06000A8F RID: 2703 RVA: 0x0005FD60 File Offset: 0x0005E160
		public CompProperties_SnowExpand()
		{
			this.compClass = typeof(CompSnowExpand);
		}

		// Token: 0x040004AE RID: 1198
		public int expandInterval = 500;

		// Token: 0x040004AF RID: 1199
		public float addAmount = 0.12f;

		// Token: 0x040004B0 RID: 1200
		public float maxRadius = 55f;
	}
}
