using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071E RID: 1822
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x040015FA RID: 5626
		public int ticksHealthy = 1000;

		// Token: 0x040015FB RID: 5627
		public int ticksNeedsMaintenance = 1000;

		// Token: 0x040015FC RID: 5628
		public int damagePerTickRare = 10;

		// Token: 0x0600282D RID: 10285 RVA: 0x0015798C File Offset: 0x00155D8C
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}
	}
}
