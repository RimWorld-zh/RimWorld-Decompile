using System;

namespace RimWorld
{
	// Token: 0x02000958 RID: 2392
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		// Token: 0x040022A0 RID: 8864
		public static BillRepeatModeDef RepeatCount;

		// Token: 0x040022A1 RID: 8865
		public static BillRepeatModeDef TargetCount;

		// Token: 0x040022A2 RID: 8866
		public static BillRepeatModeDef Forever;

		// Token: 0x06003662 RID: 13922 RVA: 0x001D0E99 File Offset: 0x001CF299
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}
	}
}
