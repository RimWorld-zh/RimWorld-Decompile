using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093A RID: 2362
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x0400209C RID: 8348
		public static SpecialThingFilterDef AllowDeadmansApparel;

		// Token: 0x0400209D RID: 8349
		public static SpecialThingFilterDef AllowNonDeadmansApparel;

		// Token: 0x06003642 RID: 13890 RVA: 0x001D0B61 File Offset: 0x001CEF61
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}
	}
}
