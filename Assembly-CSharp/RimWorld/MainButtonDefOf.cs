using System;

namespace RimWorld
{
	// Token: 0x02000924 RID: 2340
	[DefOf]
	public static class MainButtonDefOf
	{
		// Token: 0x04001FE3 RID: 8163
		public static MainButtonDef Inspect;

		// Token: 0x04001FE4 RID: 8164
		public static MainButtonDef Architect;

		// Token: 0x04001FE5 RID: 8165
		public static MainButtonDef Research;

		// Token: 0x04001FE6 RID: 8166
		public static MainButtonDef Menu;

		// Token: 0x04001FE7 RID: 8167
		public static MainButtonDef World;

		// Token: 0x0600362C RID: 13868 RVA: 0x001D09D5 File Offset: 0x001CEDD5
		static MainButtonDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MainButtonDefOf));
		}
	}
}
