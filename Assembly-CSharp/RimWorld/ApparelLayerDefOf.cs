using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000969 RID: 2409
	[DefOf]
	public static class ApparelLayerDefOf
	{
		// Token: 0x040022FE RID: 8958
		public static ApparelLayerDef OnSkin;

		// Token: 0x040022FF RID: 8959
		public static ApparelLayerDef Shell;

		// Token: 0x04002300 RID: 8960
		public static ApparelLayerDef Middle;

		// Token: 0x04002301 RID: 8961
		public static ApparelLayerDef Belt;

		// Token: 0x04002302 RID: 8962
		public static ApparelLayerDef Overhead;

		// Token: 0x06003671 RID: 13937 RVA: 0x001D0EAF File Offset: 0x001CF2AF
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}
	}
}
