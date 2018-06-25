using System;

namespace RimWorld
{
	// Token: 0x02000956 RID: 2390
	[DefOf]
	public static class FleshTypeDefOf
	{
		// Token: 0x04002298 RID: 8856
		public static FleshTypeDef Normal;

		// Token: 0x04002299 RID: 8857
		public static FleshTypeDef Mechanoid;

		// Token: 0x0400229A RID: 8858
		public static FleshTypeDef Insectoid;

		// Token: 0x06003660 RID: 13920 RVA: 0x001D0E75 File Offset: 0x001CF275
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}
	}
}
