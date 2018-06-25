using System;

namespace RimWorld
{
	// Token: 0x02000950 RID: 2384
	[DefOf]
	public static class SiteCoreDefOf
	{
		// Token: 0x04002279 RID: 8825
		public static SiteCoreDef Nothing;

		// Token: 0x0400227A RID: 8826
		public static SiteCoreDef ItemStash;

		// Token: 0x0400227B RID: 8827
		public static SiteCoreDef PreciousLump;

		// Token: 0x0400227C RID: 8828
		public static SiteCoreDef DownedRefugee;

		// Token: 0x0400227D RID: 8829
		public static SiteCoreDef PrisonerWillingToJoin;

		// Token: 0x0600365A RID: 13914 RVA: 0x001D0E09 File Offset: 0x001CF209
		static SiteCoreDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SiteCoreDefOf));
		}
	}
}
