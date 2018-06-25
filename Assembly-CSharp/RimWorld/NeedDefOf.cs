using System;

namespace RimWorld
{
	// Token: 0x02000918 RID: 2328
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x04001E9C RID: 7836
		public static NeedDef Food;

		// Token: 0x04001E9D RID: 7837
		public static NeedDef Rest;

		// Token: 0x04001E9E RID: 7838
		public static NeedDef Joy;

		// Token: 0x06003622 RID: 13858 RVA: 0x001D0CED File Offset: 0x001CF0ED
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}
	}
}
