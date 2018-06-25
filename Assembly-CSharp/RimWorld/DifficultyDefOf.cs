using System;

namespace RimWorld
{
	// Token: 0x02000933 RID: 2355
	[DefOf]
	public static class DifficultyDefOf
	{
		// Token: 0x04002048 RID: 8264
		public static DifficultyDef VeryEasy;

		// Token: 0x04002049 RID: 8265
		public static DifficultyDef Medium;

		// Token: 0x0400204A RID: 8266
		public static DifficultyDef Hard;

		// Token: 0x0600363D RID: 13885 RVA: 0x001D0BFF File Offset: 0x001CEFFF
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}
	}
}
