using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000928 RID: 2344
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x04001FEA RID: 8170
		public static RecipeDef RemoveBodyPart;

		// Token: 0x04001FEB RID: 8171
		public static RecipeDef CookMealSimple;

		// Token: 0x04001FEC RID: 8172
		public static RecipeDef InstallPegLeg;

		// Token: 0x06003632 RID: 13874 RVA: 0x001D0B39 File Offset: 0x001CEF39
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}
	}
}
