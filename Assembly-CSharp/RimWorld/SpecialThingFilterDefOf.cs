using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093E RID: 2366
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x06003647 RID: 13895 RVA: 0x001D08B1 File Offset: 0x001CECB1
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}

		// Token: 0x0400209E RID: 8350
		public static SpecialThingFilterDef AllowDeadmansApparel;

		// Token: 0x0400209F RID: 8351
		public static SpecialThingFilterDef AllowNonDeadmansApparel;
	}
}
