using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095A RID: 2394
	[DefOf]
	public static class ReservationLayerDefOf
	{
		// Token: 0x040022A6 RID: 8870
		public static ReservationLayerDef Floor;

		// Token: 0x040022A7 RID: 8871
		public static ReservationLayerDef Ceiling;

		// Token: 0x06003664 RID: 13924 RVA: 0x001D0EBD File Offset: 0x001CF2BD
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}
	}
}
