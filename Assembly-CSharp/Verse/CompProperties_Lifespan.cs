using System;

namespace Verse
{
	// Token: 0x02000B11 RID: 2833
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x040027F0 RID: 10224
		public int lifespanTicks = 100;

		// Token: 0x06003EA3 RID: 16035 RVA: 0x0020FA42 File Offset: 0x0020DE42
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}
	}
}
