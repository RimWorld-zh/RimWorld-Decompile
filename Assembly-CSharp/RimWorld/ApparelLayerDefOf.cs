using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096B RID: 2411
	[DefOf]
	public static class ApparelLayerDefOf
	{
		// Token: 0x04002306 RID: 8966
		public static ApparelLayerDef OnSkin;

		// Token: 0x04002307 RID: 8967
		public static ApparelLayerDef Shell;

		// Token: 0x04002308 RID: 8968
		public static ApparelLayerDef Middle;

		// Token: 0x04002309 RID: 8969
		public static ApparelLayerDef Belt;

		// Token: 0x0400230A RID: 8970
		public static ApparelLayerDef Overhead;

		// Token: 0x06003675 RID: 13941 RVA: 0x001D12C3 File Offset: 0x001CF6C3
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}
	}
}
