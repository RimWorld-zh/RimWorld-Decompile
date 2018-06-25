using System;

namespace RimWorld
{
	// Token: 0x0200094E RID: 2382
	[DefOf]
	public static class PawnGroupKindDefOf
	{
		// Token: 0x0400227B RID: 8827
		public static PawnGroupKindDef Combat;

		// Token: 0x0400227C RID: 8828
		public static PawnGroupKindDef Trader;

		// Token: 0x0400227D RID: 8829
		public static PawnGroupKindDef Peaceful;

		// Token: 0x0400227E RID: 8830
		public static PawnGroupKindDef FactionBase;

		// Token: 0x06003658 RID: 13912 RVA: 0x001D10B9 File Offset: 0x001CF4B9
		static PawnGroupKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnGroupKindDefOf));
		}
	}
}
