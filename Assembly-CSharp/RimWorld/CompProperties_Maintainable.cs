using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071C RID: 1820
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x0600282A RID: 10282 RVA: 0x001575DC File Offset: 0x001559DC
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}

		// Token: 0x040015F6 RID: 5622
		public int ticksHealthy = 1000;

		// Token: 0x040015F7 RID: 5623
		public int ticksNeedsMaintenance = 1000;

		// Token: 0x040015F8 RID: 5624
		public int damagePerTickRare = 10;
	}
}
