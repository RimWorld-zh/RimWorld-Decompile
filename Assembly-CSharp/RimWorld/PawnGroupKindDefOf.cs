using System;

namespace RimWorld
{
	// Token: 0x02000950 RID: 2384
	[DefOf]
	public static class PawnGroupKindDefOf
	{
		// Token: 0x06003659 RID: 13913 RVA: 0x001D09F5 File Offset: 0x001CEDF5
		static PawnGroupKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnGroupKindDefOf));
		}

		// Token: 0x04002275 RID: 8821
		public static PawnGroupKindDef Combat;

		// Token: 0x04002276 RID: 8822
		public static PawnGroupKindDef Trader;

		// Token: 0x04002277 RID: 8823
		public static PawnGroupKindDef Peaceful;

		// Token: 0x04002278 RID: 8824
		public static PawnGroupKindDef FactionBase;
	}
}
