using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093E RID: 2366
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x06003649 RID: 13897 RVA: 0x001D0979 File Offset: 0x001CED79
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
