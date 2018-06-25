using System;

namespace RimWorld
{
	// Token: 0x0200094E RID: 2382
	[DefOf]
	public static class PawnGroupKindDefOf
	{
		// Token: 0x04002274 RID: 8820
		public static PawnGroupKindDef Combat;

		// Token: 0x04002275 RID: 8821
		public static PawnGroupKindDef Trader;

		// Token: 0x04002276 RID: 8822
		public static PawnGroupKindDef Peaceful;

		// Token: 0x04002277 RID: 8823
		public static PawnGroupKindDef FactionBase;

		// Token: 0x06003658 RID: 13912 RVA: 0x001D0DE5 File Offset: 0x001CF1E5
		static PawnGroupKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnGroupKindDefOf));
		}
	}
}
