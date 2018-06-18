using System;

namespace RimWorld
{
	// Token: 0x0200095A RID: 2394
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		// Token: 0x06003665 RID: 13925 RVA: 0x001D0B71 File Offset: 0x001CEF71
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}

		// Token: 0x040022A1 RID: 8865
		public static BillRepeatModeDef RepeatCount;

		// Token: 0x040022A2 RID: 8866
		public static BillRepeatModeDef TargetCount;

		// Token: 0x040022A3 RID: 8867
		public static BillRepeatModeDef Forever;
	}
}
