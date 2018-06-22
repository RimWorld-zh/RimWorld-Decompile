using System;

namespace RimWorld
{
	// Token: 0x02000951 RID: 2385
	[DefOf]
	public static class RoadDefOf
	{
		// Token: 0x06003659 RID: 13913 RVA: 0x001D0CFF File Offset: 0x001CF0FF
		static RoadDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadDefOf));
		}

		// Token: 0x04002284 RID: 8836
		public static RoadDef DirtRoad;

		// Token: 0x04002285 RID: 8837
		public static RoadDef AncientAsphaltRoad;

		// Token: 0x04002286 RID: 8838
		public static RoadDef AncientAsphaltHighway;
	}
}
