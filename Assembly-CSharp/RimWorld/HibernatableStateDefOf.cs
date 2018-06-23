using System;

namespace RimWorld
{
	// Token: 0x0200095E RID: 2398
	[DefOf]
	public static class HibernatableStateDefOf
	{
		// Token: 0x040022C3 RID: 8899
		public static HibernatableStateDef Running;

		// Token: 0x040022C4 RID: 8900
		public static HibernatableStateDef Starting;

		// Token: 0x040022C5 RID: 8901
		public static HibernatableStateDef Hibernating;

		// Token: 0x06003666 RID: 13926 RVA: 0x001D0DE9 File Offset: 0x001CF1E9
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}
	}
}
