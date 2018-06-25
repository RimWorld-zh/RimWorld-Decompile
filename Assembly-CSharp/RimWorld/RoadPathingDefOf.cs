using System;

namespace RimWorld
{
	// Token: 0x02000954 RID: 2388
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x04002288 RID: 8840
		public static RoadPathingDef Avoid;

		// Token: 0x04002289 RID: 8841
		public static RoadPathingDef Bulldoze;

		// Token: 0x0600365E RID: 13918 RVA: 0x001D0E51 File Offset: 0x001CF251
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}
	}
}
