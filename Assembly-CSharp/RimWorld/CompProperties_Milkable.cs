using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024B RID: 587
	public class CompProperties_Milkable : CompProperties
	{
		// Token: 0x06000A81 RID: 2689 RVA: 0x0005F32B File Offset: 0x0005D72B
		public CompProperties_Milkable()
		{
			this.compClass = typeof(CompMilkable);
		}

		// Token: 0x04000497 RID: 1175
		public int milkIntervalDays;

		// Token: 0x04000498 RID: 1176
		public int milkAmount = 1;

		// Token: 0x04000499 RID: 1177
		public ThingDef milkDef;

		// Token: 0x0400049A RID: 1178
		public bool milkFemaleOnly = true;
	}
}
