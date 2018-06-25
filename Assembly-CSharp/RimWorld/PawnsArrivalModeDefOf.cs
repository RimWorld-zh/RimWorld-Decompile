using System;

namespace RimWorld
{
	// Token: 0x0200096A RID: 2410
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		// Token: 0x040022FC RID: 8956
		public static PawnsArrivalModeDef EdgeWalkIn;

		// Token: 0x040022FD RID: 8957
		public static PawnsArrivalModeDef CenterDrop;

		// Token: 0x040022FE RID: 8958
		public static PawnsArrivalModeDef EdgeDrop;

		// Token: 0x06003674 RID: 13940 RVA: 0x001D0FDD File Offset: 0x001CF3DD
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}
	}
}
