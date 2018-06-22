using System;

namespace RimWorld
{
	// Token: 0x0200095B RID: 2395
	[DefOf]
	public static class PawnTableDefOf
	{
		// Token: 0x06003663 RID: 13923 RVA: 0x001D0DB3 File Offset: 0x001CF1B3
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}

		// Token: 0x040022AC RID: 8876
		public static PawnTableDef Work;

		// Token: 0x040022AD RID: 8877
		public static PawnTableDef Assign;

		// Token: 0x040022AE RID: 8878
		public static PawnTableDef Restrict;

		// Token: 0x040022AF RID: 8879
		public static PawnTableDef Animals;

		// Token: 0x040022B0 RID: 8880
		public static PawnTableDef Wildlife;
	}
}
