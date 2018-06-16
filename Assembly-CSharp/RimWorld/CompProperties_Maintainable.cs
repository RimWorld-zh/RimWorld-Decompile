using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000720 RID: 1824
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x06002830 RID: 10288 RVA: 0x001573A8 File Offset: 0x001557A8
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
