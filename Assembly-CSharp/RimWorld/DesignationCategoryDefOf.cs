using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000918 RID: 2328
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x06003623 RID: 13859 RVA: 0x001D06CD File Offset: 0x001CEACD
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
