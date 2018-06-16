using System;

namespace RimWorld
{
	// Token: 0x0200091A RID: 2330
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x06003623 RID: 13859 RVA: 0x001D0629 File Offset: 0x001CEA29
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}

		// Token: 0x04001E97 RID: 7831
		public static NeedDef Food;

		// Token: 0x04001E98 RID: 7832
		public static NeedDef Rest;

		// Token: 0x04001E99 RID: 7833
		public static NeedDef Joy;
	}
}
