using System;

namespace RimWorld
{
	// Token: 0x02000926 RID: 2342
	[DefOf]
	public static class MainButtonDefOf
	{
		// Token: 0x04001FEA RID: 8170
		public static MainButtonDef Inspect;

		// Token: 0x04001FEB RID: 8171
		public static MainButtonDef Architect;

		// Token: 0x04001FEC RID: 8172
		public static MainButtonDef Research;

		// Token: 0x04001FED RID: 8173
		public static MainButtonDef Menu;

		// Token: 0x04001FEE RID: 8174
		public static MainButtonDef World;

		// Token: 0x06003630 RID: 13872 RVA: 0x001D0DE9 File Offset: 0x001CF1E9
		static MainButtonDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MainButtonDefOf));
		}
	}
}
