using System;

namespace RimWorld
{
	// Token: 0x02000916 RID: 2326
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x0600361E RID: 13854 RVA: 0x001D08D9 File Offset: 0x001CECD9
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}

		// Token: 0x04001E95 RID: 7829
		public static NeedDef Food;

		// Token: 0x04001E96 RID: 7830
		public static NeedDef Rest;

		// Token: 0x04001E97 RID: 7831
		public static NeedDef Joy;
	}
}
