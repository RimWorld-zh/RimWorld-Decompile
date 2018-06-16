using System;

namespace RimWorld
{
	// Token: 0x02000928 RID: 2344
	[DefOf]
	public static class MainButtonDefOf
	{
		// Token: 0x06003631 RID: 13873 RVA: 0x001D0725 File Offset: 0x001CEB25
		static MainButtonDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MainButtonDefOf));
		}

		// Token: 0x04001FE5 RID: 8165
		public static MainButtonDef Inspect;

		// Token: 0x04001FE6 RID: 8166
		public static MainButtonDef Architect;

		// Token: 0x04001FE7 RID: 8167
		public static MainButtonDef Research;

		// Token: 0x04001FE8 RID: 8168
		public static MainButtonDef Menu;

		// Token: 0x04001FE9 RID: 8169
		public static MainButtonDef World;
	}
}
