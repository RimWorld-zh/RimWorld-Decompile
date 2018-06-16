using System;

namespace RimWorld
{
	// Token: 0x02000958 RID: 2392
	[DefOf]
	public static class FleshTypeDefOf
	{
		// Token: 0x06003661 RID: 13921 RVA: 0x001D0A85 File Offset: 0x001CEE85
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}

		// Token: 0x04002299 RID: 8857
		public static FleshTypeDef Normal;

		// Token: 0x0400229A RID: 8858
		public static FleshTypeDef Mechanoid;

		// Token: 0x0400229B RID: 8859
		public static FleshTypeDef Insectoid;
	}
}
