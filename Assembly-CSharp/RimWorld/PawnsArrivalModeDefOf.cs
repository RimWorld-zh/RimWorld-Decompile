using System;

namespace RimWorld
{
	// Token: 0x0200096A RID: 2410
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		// Token: 0x04002303 RID: 8963
		public static PawnsArrivalModeDef EdgeWalkIn;

		// Token: 0x04002304 RID: 8964
		public static PawnsArrivalModeDef CenterDrop;

		// Token: 0x04002305 RID: 8965
		public static PawnsArrivalModeDef EdgeDrop;

		// Token: 0x06003674 RID: 13940 RVA: 0x001D12B1 File Offset: 0x001CF6B1
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}
	}
}
