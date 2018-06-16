using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000918 RID: 2328
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x06003621 RID: 13857 RVA: 0x001D0605 File Offset: 0x001CEA05
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}

		// Token: 0x04001E81 RID: 7809
		public static DesignationCategoryDef Production;

		// Token: 0x04001E82 RID: 7810
		public static DesignationCategoryDef Structure;

		// Token: 0x04001E83 RID: 7811
		public static DesignationCategoryDef Security;
	}
}
