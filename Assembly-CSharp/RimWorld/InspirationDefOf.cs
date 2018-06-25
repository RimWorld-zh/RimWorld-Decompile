using System;

namespace RimWorld
{
	// Token: 0x02000964 RID: 2404
	[DefOf]
	public static class InspirationDefOf
	{
		// Token: 0x040022DB RID: 8923
		public static InspirationDef Inspired_Trade;

		// Token: 0x040022DC RID: 8924
		public static InspirationDef Inspired_Recruitment;

		// Token: 0x040022DD RID: 8925
		public static InspirationDef Inspired_Surgery;

		// Token: 0x040022DE RID: 8926
		public static InspirationDef Inspired_Creativity;

		// Token: 0x0600366E RID: 13934 RVA: 0x001D1245 File Offset: 0x001CF645
		static InspirationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InspirationDefOf));
		}
	}
}
