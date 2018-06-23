using System;

namespace RimWorld
{
	// Token: 0x02000962 RID: 2402
	[DefOf]
	public static class InspirationDefOf
	{
		// Token: 0x040022D3 RID: 8915
		public static InspirationDef Inspired_Trade;

		// Token: 0x040022D4 RID: 8916
		public static InspirationDef Inspired_Recruitment;

		// Token: 0x040022D5 RID: 8917
		public static InspirationDef Inspired_Surgery;

		// Token: 0x040022D6 RID: 8918
		public static InspirationDef Inspired_Creativity;

		// Token: 0x0600366A RID: 13930 RVA: 0x001D0E31 File Offset: 0x001CF231
		static InspirationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InspirationDefOf));
		}
	}
}
