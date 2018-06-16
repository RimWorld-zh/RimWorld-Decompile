using System;

namespace Verse
{
	// Token: 0x02000B13 RID: 2835
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x06003EA1 RID: 16033 RVA: 0x0020F506 File Offset: 0x0020D906
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}

		// Token: 0x040027F3 RID: 10227
		public int lifespanTicks = 100;
	}
}
