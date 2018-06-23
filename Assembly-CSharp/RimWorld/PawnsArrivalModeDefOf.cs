using System;

namespace RimWorld
{
	// Token: 0x02000968 RID: 2408
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		// Token: 0x040022FB RID: 8955
		public static PawnsArrivalModeDef EdgeWalkIn;

		// Token: 0x040022FC RID: 8956
		public static PawnsArrivalModeDef CenterDrop;

		// Token: 0x040022FD RID: 8957
		public static PawnsArrivalModeDef EdgeDrop;

		// Token: 0x06003670 RID: 13936 RVA: 0x001D0E9D File Offset: 0x001CF29D
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}
	}
}
