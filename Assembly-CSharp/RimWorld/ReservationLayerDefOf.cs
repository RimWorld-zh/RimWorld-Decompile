using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000958 RID: 2392
	[DefOf]
	public static class ReservationLayerDefOf
	{
		// Token: 0x06003660 RID: 13920 RVA: 0x001D0D7D File Offset: 0x001CF17D
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}

		// Token: 0x040022A5 RID: 8869
		public static ReservationLayerDef Floor;

		// Token: 0x040022A6 RID: 8870
		public static ReservationLayerDef Ceiling;
	}
}
