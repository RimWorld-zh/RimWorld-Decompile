using System;

namespace RimWorld
{
	// Token: 0x0200094F RID: 2383
	[DefOf]
	public static class SitePartDefOf
	{
		// Token: 0x0400227D RID: 8829
		public static SitePartDef Outpost;

		// Token: 0x0400227E RID: 8830
		public static SitePartDef Turrets;

		// Token: 0x0400227F RID: 8831
		public static SitePartDef Manhunters;

		// Token: 0x04002280 RID: 8832
		public static SitePartDef SleepingMechanoids;

		// Token: 0x04002281 RID: 8833
		public static SitePartDef AmbushHidden;

		// Token: 0x04002282 RID: 8834
		public static SitePartDef AmbushEdge;

		// Token: 0x06003657 RID: 13911 RVA: 0x001D0CDB File Offset: 0x001CF0DB
		static SitePartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SitePartDefOf));
		}
	}
}
