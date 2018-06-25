using System;

namespace RimWorld
{
	// Token: 0x02000956 RID: 2390
	[DefOf]
	public static class FleshTypeDefOf
	{
		// Token: 0x0400229F RID: 8863
		public static FleshTypeDef Normal;

		// Token: 0x040022A0 RID: 8864
		public static FleshTypeDef Mechanoid;

		// Token: 0x040022A1 RID: 8865
		public static FleshTypeDef Insectoid;

		// Token: 0x06003660 RID: 13920 RVA: 0x001D1149 File Offset: 0x001CF549
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}
	}
}
