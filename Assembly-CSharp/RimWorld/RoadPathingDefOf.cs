using System;

namespace RimWorld
{
	// Token: 0x02000956 RID: 2390
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x0600365F RID: 13919 RVA: 0x001D0A61 File Offset: 0x001CEE61
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
