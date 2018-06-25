using System;

namespace RimWorld
{
	// Token: 0x02000933 RID: 2355
	[DefOf]
	public static class DifficultyDefOf
	{
		// Token: 0x0400204F RID: 8271
		public static DifficultyDef Easy;

		// Token: 0x04002050 RID: 8272
		public static DifficultyDef Hard;

		// Token: 0x04002051 RID: 8273
		public static DifficultyDef ExtraHard;

		// Token: 0x0600363D RID: 13885 RVA: 0x001D0ED3 File Offset: 0x001CF2D3
		static DifficultyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DifficultyDefOf));
		}
	}
}
