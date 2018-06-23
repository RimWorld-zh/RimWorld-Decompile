using System;

namespace RimWorld
{
	// Token: 0x02000931 RID: 2353
	[DefOf]
	public static class DifficultyDefOf
	{
		// Token: 0x04002048 RID: 8264
		public static DifficultyDef VeryEasy;

		// Token: 0x04002049 RID: 8265
		public static DifficultyDef Medium;

		// Token: 0x0400204A RID: 8266
		public static DifficultyDef Hard;

		// Token: 0x06003639 RID: 13881 RVA: 0x001D0ABF File Offset: 0x001CEEBF
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}
	}
}
