using System;

namespace RimWorld
{
	// Token: 0x02000955 RID: 2389
	[DefOf]
	public static class RoadDefOf
	{
		// Token: 0x06003660 RID: 13920 RVA: 0x001D0B17 File Offset: 0x001CEF17
		static RoadDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadDefOf));
		}

		// Token: 0x04002286 RID: 8838
		public static RoadDef DirtRoad;

		// Token: 0x04002287 RID: 8839
		public static RoadDef AncientAsphaltRoad;

		// Token: 0x04002288 RID: 8840
		public static RoadDef AncientAsphaltHighway;
	}
}
