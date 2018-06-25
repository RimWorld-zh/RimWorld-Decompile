using System;

namespace RimWorld
{
	// Token: 0x02000926 RID: 2342
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

		// Token: 0x06003630 RID: 13872 RVA: 0x001D0B15 File Offset: 0x001CEF15
		static MainButtonDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MainButtonDefOf));
		}
	}
}
