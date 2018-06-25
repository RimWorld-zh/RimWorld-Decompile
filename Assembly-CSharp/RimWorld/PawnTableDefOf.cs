using System;

namespace RimWorld
{
	// Token: 0x0200095D RID: 2397
	[DefOf]
	public static class PawnTableDefOf
	{
		// Token: 0x040022B4 RID: 8884
		public static PawnTableDef Work;

		// Token: 0x040022B5 RID: 8885
		public static PawnTableDef Assign;

		// Token: 0x040022B6 RID: 8886
		public static PawnTableDef Restrict;

		// Token: 0x040022B7 RID: 8887
		public static PawnTableDef Animals;

		// Token: 0x040022B8 RID: 8888
		public static PawnTableDef Wildlife;

		// Token: 0x06003667 RID: 13927 RVA: 0x001D11C7 File Offset: 0x001CF5C7
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}
	}
}
