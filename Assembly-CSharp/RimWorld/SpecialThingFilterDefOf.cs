using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093C RID: 2364
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x0400209D RID: 8349
		public static SpecialThingFilterDef AllowDeadmansApparel;

		// Token: 0x0400209E RID: 8350
		public static SpecialThingFilterDef AllowNonDeadmansApparel;

		// Token: 0x06003646 RID: 13894 RVA: 0x001D0CA1 File Offset: 0x001CF0A1
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}
	}
}
