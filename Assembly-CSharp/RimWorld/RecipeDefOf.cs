using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092A RID: 2346
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x06003635 RID: 13877 RVA: 0x001D0811 File Offset: 0x001CEC11
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}

		// Token: 0x04001FEC RID: 8172
		public static RecipeDef RemoveBodyPart;

		// Token: 0x04001FED RID: 8173
		public static RecipeDef CookMealSimple;

		// Token: 0x04001FEE RID: 8174
		public static RecipeDef InstallPegLeg;
	}
}
