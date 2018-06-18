using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000720 RID: 1824
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x06002832 RID: 10290 RVA: 0x00157420 File Offset: 0x00155820
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}

		// Token: 0x040015F8 RID: 5624
		public int ticksHealthy = 1000;

		// Token: 0x040015F9 RID: 5625
		public int ticksNeedsMaintenance = 1000;

		// Token: 0x040015FA RID: 5626
		public int damagePerTickRare = 10;
	}
}
