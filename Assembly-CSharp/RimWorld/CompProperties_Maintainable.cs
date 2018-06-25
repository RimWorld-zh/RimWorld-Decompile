using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071E RID: 1822
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x040015F6 RID: 5622
		public int ticksHealthy = 1000;

		// Token: 0x040015F7 RID: 5623
		public int ticksNeedsMaintenance = 1000;

		// Token: 0x040015F8 RID: 5624
		public int damagePerTickRare = 10;

		// Token: 0x0600282E RID: 10286 RVA: 0x0015772C File Offset: 0x00155B2C
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}
	}
}
