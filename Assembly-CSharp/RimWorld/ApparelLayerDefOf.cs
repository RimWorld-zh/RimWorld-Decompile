using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096B RID: 2411
	[DefOf]
	public static class ApparelLayerDefOf
	{
		// Token: 0x040022FF RID: 8959
		public static ApparelLayerDef OnSkin;

		// Token: 0x04002300 RID: 8960
		public static ApparelLayerDef Shell;

		// Token: 0x04002301 RID: 8961
		public static ApparelLayerDef Middle;

		// Token: 0x04002302 RID: 8962
		public static ApparelLayerDef Belt;

		// Token: 0x04002303 RID: 8963
		public static ApparelLayerDef Overhead;

		// Token: 0x06003675 RID: 13941 RVA: 0x001D0FEF File Offset: 0x001CF3EF
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}
	}
}
