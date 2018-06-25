using System;

namespace RimWorld
{
	// Token: 0x02000951 RID: 2385
	[DefOf]
	public static class SitePartDefOf
	{
		// Token: 0x04002285 RID: 8837
		public static SitePartDef Outpost;

		// Token: 0x04002286 RID: 8838
		public static SitePartDef Turrets;

		// Token: 0x04002287 RID: 8839
		public static SitePartDef Manhunters;

		// Token: 0x04002288 RID: 8840
		public static SitePartDef SleepingMechanoids;

		// Token: 0x04002289 RID: 8841
		public static SitePartDef AmbushHidden;

		// Token: 0x0400228A RID: 8842
		public static SitePartDef AmbushEdge;

		// Token: 0x0600365B RID: 13915 RVA: 0x001D10EF File Offset: 0x001CF4EF
		static SitePartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SitePartDefOf));
		}
	}
}
