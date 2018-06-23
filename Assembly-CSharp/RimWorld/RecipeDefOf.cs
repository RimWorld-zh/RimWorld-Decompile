using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000926 RID: 2342
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x04001FEA RID: 8170
		public static RecipeDef RemoveBodyPart;

		// Token: 0x04001FEB RID: 8171
		public static RecipeDef CookMealSimple;

		// Token: 0x04001FEC RID: 8172
		public static RecipeDef InstallPegLeg;

		// Token: 0x0600362E RID: 13870 RVA: 0x001D09F9 File Offset: 0x001CEDF9
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}
	}
}
