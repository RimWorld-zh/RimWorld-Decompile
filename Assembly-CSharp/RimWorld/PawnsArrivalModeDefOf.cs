using System;

namespace RimWorld
{
	// Token: 0x0200096C RID: 2412
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		// Token: 0x06003675 RID: 13941 RVA: 0x001D0BED File Offset: 0x001CEFED
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}

		// Token: 0x040022FD RID: 8957
		public static PawnsArrivalModeDef EdgeWalkIn;

		// Token: 0x040022FE RID: 8958
		public static PawnsArrivalModeDef CenterDrop;

		// Token: 0x040022FF RID: 8959
		public static PawnsArrivalModeDef EdgeDrop;
	}
}
