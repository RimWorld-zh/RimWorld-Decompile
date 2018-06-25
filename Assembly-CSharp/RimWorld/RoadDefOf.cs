using System;

namespace RimWorld
{
	// Token: 0x02000953 RID: 2387
	[DefOf]
	public static class RoadDefOf
	{
		// Token: 0x0400228C RID: 8844
		public static RoadDef DirtRoad;

		// Token: 0x0400228D RID: 8845
		public static RoadDef AncientAsphaltRoad;

		// Token: 0x0400228E RID: 8846
		public static RoadDef AncientAsphaltHighway;

		// Token: 0x0600365D RID: 13917 RVA: 0x001D1113 File Offset: 0x001CF513
		static RoadDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadDefOf));
		}
	}
}
