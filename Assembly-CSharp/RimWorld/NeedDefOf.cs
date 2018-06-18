using System;

namespace RimWorld
{
	// Token: 0x0200091A RID: 2330
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x06003625 RID: 13861 RVA: 0x001D06F1 File Offset: 0x001CEAF1
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
