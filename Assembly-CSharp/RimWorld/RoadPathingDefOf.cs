using System;

namespace RimWorld
{
	// Token: 0x02000956 RID: 2390
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x06003661 RID: 13921 RVA: 0x001D0B29 File Offset: 0x001CEF29
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}

		// Token: 0x04002289 RID: 8841
		public static RoadPathingDef Avoid;

		// Token: 0x0400228A RID: 8842
		public static RoadPathingDef Bulldoze;
	}
}
