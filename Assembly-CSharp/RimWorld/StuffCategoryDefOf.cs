using System;

namespace RimWorld
{
	// Token: 0x02000939 RID: 2361
	[DefOf]
	public static class StuffCategoryDefOf
	{
		// Token: 0x04002062 RID: 8290
		public static StuffCategoryDef Metallic;

		// Token: 0x04002063 RID: 8291
		public static StuffCategoryDef Woody;

		// Token: 0x04002064 RID: 8292
		public static StuffCategoryDef Stony;

		// Token: 0x04002065 RID: 8293
		public static StuffCategoryDef Fabric;

		// Token: 0x04002066 RID: 8294
		public static StuffCategoryDef Leathery;

		// Token: 0x06003643 RID: 13891 RVA: 0x001D0C6B File Offset: 0x001CF06B
		static StuffCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StuffCategoryDefOf));
		}
	}
}
