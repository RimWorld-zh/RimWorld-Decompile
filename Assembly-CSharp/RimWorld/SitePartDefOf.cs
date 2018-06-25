using System;

namespace RimWorld
{
	// Token: 0x02000951 RID: 2385
	[DefOf]
	public static class SitePartDefOf
	{
		// Token: 0x0400227E RID: 8830
		public static SitePartDef Outpost;

		// Token: 0x0400227F RID: 8831
		public static SitePartDef Turrets;

		// Token: 0x04002280 RID: 8832
		public static SitePartDef Manhunters;

		// Token: 0x04002281 RID: 8833
		public static SitePartDef SleepingMechanoids;

		// Token: 0x04002282 RID: 8834
		public static SitePartDef AmbushHidden;

		// Token: 0x04002283 RID: 8835
		public static SitePartDef AmbushEdge;

		// Token: 0x0600365B RID: 13915 RVA: 0x001D0E1B File Offset: 0x001CF21B
		static SitePartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SitePartDefOf));
		}
	}
}
