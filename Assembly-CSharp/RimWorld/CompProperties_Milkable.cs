using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200024B RID: 587
	public class CompProperties_Milkable : CompProperties
	{
		// Token: 0x04000495 RID: 1173
		public int milkIntervalDays;

		// Token: 0x04000496 RID: 1174
		public int milkAmount = 1;

		// Token: 0x04000497 RID: 1175
		public ThingDef milkDef;

		// Token: 0x04000498 RID: 1176
		public bool milkFemaleOnly = true;

		// Token: 0x06000A7F RID: 2687 RVA: 0x0005F387 File Offset: 0x0005D787
		public CompProperties_Milkable()
		{
			this.compClass = typeof(CompMilkable);
		}
	}
}
