using System;

namespace RimWorld
{
	// Token: 0x02000954 RID: 2388
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x0400228F RID: 8847
		public static RoadPathingDef Avoid;

		// Token: 0x04002290 RID: 8848
		public static RoadPathingDef Bulldoze;

		// Token: 0x0600365E RID: 13918 RVA: 0x001D1125 File Offset: 0x001CF525
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}
	}
}
