using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000928 RID: 2344
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x04001FF1 RID: 8177
		public static RecipeDef RemoveBodyPart;

		// Token: 0x04001FF2 RID: 8178
		public static RecipeDef CookMealSimple;

		// Token: 0x04001FF3 RID: 8179
		public static RecipeDef InstallPegLeg;

		// Token: 0x06003632 RID: 13874 RVA: 0x001D0E0D File Offset: 0x001CF20D
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}
	}
}
