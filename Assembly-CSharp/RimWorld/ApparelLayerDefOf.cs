using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096D RID: 2413
	[DefOf]
	public static class ApparelLayerDefOf
	{
		// Token: 0x06003676 RID: 13942 RVA: 0x001D0BFF File Offset: 0x001CEFFF
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}

		// Token: 0x04002300 RID: 8960
		public static ApparelLayerDef OnSkin;

		// Token: 0x04002301 RID: 8961
		public static ApparelLayerDef Shell;

		// Token: 0x04002302 RID: 8962
		public static ApparelLayerDef Middle;

		// Token: 0x04002303 RID: 8963
		public static ApparelLayerDef Belt;

		// Token: 0x04002304 RID: 8964
		public static ApparelLayerDef Overhead;
	}
}
