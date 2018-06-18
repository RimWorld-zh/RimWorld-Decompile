using System;

namespace RimWorld
{
	// Token: 0x0200095F RID: 2399
	[DefOf]
	public static class PawnTableDefOf
	{
		// Token: 0x0600366A RID: 13930 RVA: 0x001D0BCB File Offset: 0x001CEFCB
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}

		// Token: 0x040022AE RID: 8878
		public static PawnTableDef Work;

		// Token: 0x040022AF RID: 8879
		public static PawnTableDef Assign;

		// Token: 0x040022B0 RID: 8880
		public static PawnTableDef Restrict;

		// Token: 0x040022B1 RID: 8881
		public static PawnTableDef Animals;

		// Token: 0x040022B2 RID: 8882
		public static PawnTableDef Wildlife;
	}
}
