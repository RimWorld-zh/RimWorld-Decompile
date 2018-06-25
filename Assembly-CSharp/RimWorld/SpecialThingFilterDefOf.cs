using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093C RID: 2364
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x040020A4 RID: 8356
		public static SpecialThingFilterDef AllowDeadmansApparel;

		// Token: 0x040020A5 RID: 8357
		public static SpecialThingFilterDef AllowNonDeadmansApparel;

		// Token: 0x06003646 RID: 13894 RVA: 0x001D0F75 File Offset: 0x001CF375
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}
	}
}
