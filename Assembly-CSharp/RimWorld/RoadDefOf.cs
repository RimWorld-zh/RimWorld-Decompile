using System;

namespace RimWorld
{
	// Token: 0x02000953 RID: 2387
	[DefOf]
	public static class RoadDefOf
	{
		// Token: 0x04002285 RID: 8837
		public static RoadDef DirtRoad;

		// Token: 0x04002286 RID: 8838
		public static RoadDef AncientAsphaltRoad;

		// Token: 0x04002287 RID: 8839
		public static RoadDef AncientAsphaltHighway;

		// Token: 0x0600365D RID: 13917 RVA: 0x001D0E3F File Offset: 0x001CF23F
		static RoadDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadDefOf));
		}
	}
}
