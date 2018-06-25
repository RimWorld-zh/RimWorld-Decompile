using System;

namespace RimWorld
{
	// Token: 0x02000918 RID: 2328
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x04001E95 RID: 7829
		public static NeedDef Food;

		// Token: 0x04001E96 RID: 7830
		public static NeedDef Rest;

		// Token: 0x04001E97 RID: 7831
		public static NeedDef Joy;

		// Token: 0x06003622 RID: 13858 RVA: 0x001D0A19 File Offset: 0x001CEE19
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}
	}
}
