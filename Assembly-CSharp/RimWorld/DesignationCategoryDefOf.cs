using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000914 RID: 2324
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x04001E7F RID: 7807
		public static DesignationCategoryDef Production;

		// Token: 0x04001E80 RID: 7808
		public static DesignationCategoryDef Structure;

		// Token: 0x04001E81 RID: 7809
		public static DesignationCategoryDef Security;

		// Token: 0x0600361C RID: 13852 RVA: 0x001D08B5 File Offset: 0x001CECB5
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}
	}
}
