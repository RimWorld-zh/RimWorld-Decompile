using System;

namespace RimWorld
{
	// Token: 0x0200094C RID: 2380
	[DefOf]
	public static class PawnGroupKindDefOf
	{
		// Token: 0x06003654 RID: 13908 RVA: 0x001D0CA5 File Offset: 0x001CF0A5
		static PawnGroupKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnGroupKindDefOf));
		}

		// Token: 0x04002273 RID: 8819
		public static PawnGroupKindDef Combat;

		// Token: 0x04002274 RID: 8820
		public static PawnGroupKindDef Trader;

		// Token: 0x04002275 RID: 8821
		public static PawnGroupKindDef Peaceful;

		// Token: 0x04002276 RID: 8822
		public static PawnGroupKindDef FactionBase;
	}
}
