using System;

namespace RimWorld
{
	// Token: 0x0200095D RID: 2397
	[DefOf]
	public static class PawnTableDefOf
	{
		// Token: 0x040022AD RID: 8877
		public static PawnTableDef Work;

		// Token: 0x040022AE RID: 8878
		public static PawnTableDef Assign;

		// Token: 0x040022AF RID: 8879
		public static PawnTableDef Restrict;

		// Token: 0x040022B0 RID: 8880
		public static PawnTableDef Animals;

		// Token: 0x040022B1 RID: 8881
		public static PawnTableDef Wildlife;

		// Token: 0x06003667 RID: 13927 RVA: 0x001D0EF3 File Offset: 0x001CF2F3
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}
	}
}
