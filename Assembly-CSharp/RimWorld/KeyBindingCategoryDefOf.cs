using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000949 RID: 2377
	[DefOf]
	public static class KeyBindingCategoryDefOf
	{
		// Token: 0x04002234 RID: 8756
		public static KeyBindingCategoryDef MainTabs;

		// Token: 0x06003653 RID: 13907 RVA: 0x001D0D8B File Offset: 0x001CF18B
		static KeyBindingCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(KeyBindingCategoryDefOf));
		}
	}
}
