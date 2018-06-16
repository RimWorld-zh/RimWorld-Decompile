using System;

namespace RimWorld
{
	// Token: 0x02000955 RID: 2389
	[DefOf]
	public static class RoadDefOf
	{
		// Token: 0x0600365E RID: 13918 RVA: 0x001D0A4F File Offset: 0x001CEE4F
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
