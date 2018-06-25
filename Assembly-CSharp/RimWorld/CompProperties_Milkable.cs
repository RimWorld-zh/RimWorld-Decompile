using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024D RID: 589
	public class CompProperties_Milkable : CompProperties
	{
		// Token: 0x04000497 RID: 1175
		public int milkIntervalDays;

		// Token: 0x04000498 RID: 1176
		public int milkAmount = 1;

		// Token: 0x04000499 RID: 1177
		public ThingDef milkDef;

		// Token: 0x0400049A RID: 1178
		public bool milkFemaleOnly = true;

		// Token: 0x06000A82 RID: 2690 RVA: 0x0005F4D3 File Offset: 0x0005D8D3
		public CompProperties_Milkable()
		{
			this.compClass = typeof(CompMilkable);
		}
	}
}
