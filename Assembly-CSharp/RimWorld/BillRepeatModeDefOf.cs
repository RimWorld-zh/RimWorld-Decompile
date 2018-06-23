using System;

namespace RimWorld
{
	// Token: 0x02000956 RID: 2390
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		// Token: 0x0400229F RID: 8863
		public static BillRepeatModeDef RepeatCount;

		// Token: 0x040022A0 RID: 8864
		public static BillRepeatModeDef TargetCount;

		// Token: 0x040022A1 RID: 8865
		public static BillRepeatModeDef Forever;

		// Token: 0x0600365E RID: 13918 RVA: 0x001D0D59 File Offset: 0x001CF159
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}
	}
}
