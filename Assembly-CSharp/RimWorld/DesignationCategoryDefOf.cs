using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000916 RID: 2326
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x04001E7F RID: 7807
		public static DesignationCategoryDef Production;

		// Token: 0x04001E80 RID: 7808
		public static DesignationCategoryDef Structure;

		// Token: 0x04001E81 RID: 7809
		public static DesignationCategoryDef Security;

		// Token: 0x06003620 RID: 13856 RVA: 0x001D09F5 File Offset: 0x001CEDF5
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}
	}
}
