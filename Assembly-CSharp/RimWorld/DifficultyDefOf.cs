using System;

namespace RimWorld
{
	// Token: 0x02000935 RID: 2357
	[DefOf]
	public static class DifficultyDefOf
	{
		// Token: 0x0600363E RID: 13886 RVA: 0x001D080F File Offset: 0x001CEC0F
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}

		// Token: 0x0400204A RID: 8266
		public static DifficultyDef VeryEasy;

		// Token: 0x0400204B RID: 8267
		public static DifficultyDef Medium;

		// Token: 0x0400204C RID: 8268
		public static DifficultyDef Hard;
	}
}
