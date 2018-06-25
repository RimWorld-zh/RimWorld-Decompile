using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000916 RID: 2326
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x04001E86 RID: 7814
		public static DesignationCategoryDef Production;

		// Token: 0x04001E87 RID: 7815
		public static DesignationCategoryDef Structure;

		// Token: 0x04001E88 RID: 7816
		public static DesignationCategoryDef Security;

		// Token: 0x06003620 RID: 13856 RVA: 0x001D0CC9 File Offset: 0x001CF0C9
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}
	}
}
