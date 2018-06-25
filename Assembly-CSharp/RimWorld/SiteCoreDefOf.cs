using System;

namespace RimWorld
{
	// Token: 0x02000950 RID: 2384
	[DefOf]
	public static class SiteCoreDefOf
	{
		// Token: 0x04002280 RID: 8832
		public static SiteCoreDef Nothing;

		// Token: 0x04002281 RID: 8833
		public static SiteCoreDef ItemStash;

		// Token: 0x04002282 RID: 8834
		public static SiteCoreDef PreciousLump;

		// Token: 0x04002283 RID: 8835
		public static SiteCoreDef DownedRefugee;

		// Token: 0x04002284 RID: 8836
		public static SiteCoreDef PrisonerWillingToJoin;

		// Token: 0x0600365A RID: 13914 RVA: 0x001D10DD File Offset: 0x001CF4DD
		static SiteCoreDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SiteCoreDefOf));
		}
	}
}
