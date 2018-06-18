using System;

namespace Verse
{
	// Token: 0x02000B13 RID: 2835
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x06003EA3 RID: 16035 RVA: 0x0020F5DA File Offset: 0x0020D9DA
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}

		// Token: 0x040027F3 RID: 10227
		public int lifespanTicks = 100;
	}
}
