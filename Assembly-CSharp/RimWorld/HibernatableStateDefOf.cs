using System;

namespace RimWorld
{
	// Token: 0x02000960 RID: 2400
	[DefOf]
	public static class HibernatableStateDefOf
	{
		// Token: 0x040022C4 RID: 8900
		public static HibernatableStateDef Running;

		// Token: 0x040022C5 RID: 8901
		public static HibernatableStateDef Starting;

		// Token: 0x040022C6 RID: 8902
		public static HibernatableStateDef Hibernating;

		// Token: 0x0600366A RID: 13930 RVA: 0x001D0F29 File Offset: 0x001CF329
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}
	}
}
