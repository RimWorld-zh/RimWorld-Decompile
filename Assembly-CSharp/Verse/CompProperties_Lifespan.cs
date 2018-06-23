using System;

namespace Verse
{
	// Token: 0x02000B0F RID: 2831
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x040027EF RID: 10223
		public int lifespanTicks = 100;

		// Token: 0x06003E9F RID: 16031 RVA: 0x0020F916 File Offset: 0x0020DD16
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}
	}
}
