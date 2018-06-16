using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000252 RID: 594
	public class CompProperties_SnowExpand : CompProperties
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x0005FD04 File Offset: 0x0005E104
		public CompProperties_SnowExpand()
		{
			this.compClass = typeof(CompSnowExpand);
		}

		// Token: 0x040004B0 RID: 1200
		public int expandInterval = 500;

		// Token: 0x040004B1 RID: 1201
		public float addAmount = 0.12f;

		// Token: 0x040004B2 RID: 1202
		public float maxRadius = 55f;
	}
}
