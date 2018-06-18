using System;

namespace RimWorld
{
	// Token: 0x02000962 RID: 2402
	[DefOf]
	public static class HibernatableStateDefOf
	{
		// Token: 0x0600366D RID: 13933 RVA: 0x001D0C01 File Offset: 0x001CF001
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}

		// Token: 0x040022C5 RID: 8901
		public static HibernatableStateDef Running;

		// Token: 0x040022C6 RID: 8902
		public static HibernatableStateDef Starting;

		// Token: 0x040022C7 RID: 8903
		public static HibernatableStateDef Hibernating;
	}
}
