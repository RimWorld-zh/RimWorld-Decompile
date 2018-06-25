using System;

namespace RimWorld
{
	// Token: 0x02000958 RID: 2392
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		// Token: 0x040022A7 RID: 8871
		public static BillRepeatModeDef RepeatCount;

		// Token: 0x040022A8 RID: 8872
		public static BillRepeatModeDef TargetCount;

		// Token: 0x040022A9 RID: 8873
		public static BillRepeatModeDef Forever;

		// Token: 0x06003662 RID: 13922 RVA: 0x001D116D File Offset: 0x001CF56D
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}
	}
}
