using System;

namespace RimWorld
{
	// Token: 0x02000939 RID: 2361
	[DefOf]
	public static class StuffCategoryDefOf
	{
		// Token: 0x04002069 RID: 8297
		public static StuffCategoryDef Metallic;

		// Token: 0x0400206A RID: 8298
		public static StuffCategoryDef Woody;

		// Token: 0x0400206B RID: 8299
		public static StuffCategoryDef Stony;

		// Token: 0x0400206C RID: 8300
		public static StuffCategoryDef Fabric;

		// Token: 0x0400206D RID: 8301
		public static StuffCategoryDef Leathery;

		// Token: 0x06003643 RID: 13891 RVA: 0x001D0F3F File Offset: 0x001CF33F
		static StuffCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StuffCategoryDefOf));
		}
	}
}
