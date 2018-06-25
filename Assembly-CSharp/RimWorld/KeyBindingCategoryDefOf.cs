using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000949 RID: 2377
	[DefOf]
	public static class KeyBindingCategoryDefOf
	{
		// Token: 0x0400223B RID: 8763
		public static KeyBindingCategoryDef MainTabs;

		// Token: 0x06003653 RID: 13907 RVA: 0x001D105F File Offset: 0x001CF45F
		static KeyBindingCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(KeyBindingCategoryDefOf));
		}
	}
}
