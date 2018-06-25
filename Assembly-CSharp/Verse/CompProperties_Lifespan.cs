using System;

namespace Verse
{
	// Token: 0x02000B12 RID: 2834
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x040027F7 RID: 10231
		public int lifespanTicks = 100;

		// Token: 0x06003EA3 RID: 16035 RVA: 0x0020FD22 File Offset: 0x0020E122
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}
	}
}
