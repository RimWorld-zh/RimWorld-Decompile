using System;

namespace RimWorld
{
	// Token: 0x02000960 RID: 2400
	[DefOf]
	public static class HibernatableStateDefOf
	{
		// Token: 0x040022CB RID: 8907
		public static HibernatableStateDef Running;

		// Token: 0x040022CC RID: 8908
		public static HibernatableStateDef Starting;

		// Token: 0x040022CD RID: 8909
		public static HibernatableStateDef Hibernating;

		// Token: 0x0600366A RID: 13930 RVA: 0x001D11FD File Offset: 0x001CF5FD
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}
	}
}
