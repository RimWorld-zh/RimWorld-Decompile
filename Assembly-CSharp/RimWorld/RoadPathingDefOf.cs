using System;

namespace RimWorld
{
	// Token: 0x02000952 RID: 2386
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x04002287 RID: 8839
		public static RoadPathingDef Avoid;

		// Token: 0x04002288 RID: 8840
		public static RoadPathingDef Bulldoze;

		// Token: 0x0600365A RID: 13914 RVA: 0x001D0D11 File Offset: 0x001CF111
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}
	}
}
