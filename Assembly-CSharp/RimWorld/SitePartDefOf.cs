using System;

namespace RimWorld
{
	// Token: 0x02000953 RID: 2387
	[DefOf]
	public static class SitePartDefOf
	{
		// Token: 0x0600365C RID: 13916 RVA: 0x001D0A2B File Offset: 0x001CEE2B
		static SitePartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SitePartDefOf));
		}

		// Token: 0x0400227F RID: 8831
		public static SitePartDef Outpost;

		// Token: 0x04002280 RID: 8832
		public static SitePartDef Turrets;

		// Token: 0x04002281 RID: 8833
		public static SitePartDef Manhunters;

		// Token: 0x04002282 RID: 8834
		public static SitePartDef SleepingMechanoids;

		// Token: 0x04002283 RID: 8835
		public static SitePartDef AmbushHidden;

		// Token: 0x04002284 RID: 8836
		public static SitePartDef AmbushEdge;
	}
}
