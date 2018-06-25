using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095A RID: 2394
	[DefOf]
	public static class ReservationLayerDefOf
	{
		// Token: 0x040022AD RID: 8877
		public static ReservationLayerDef Floor;

		// Token: 0x040022AE RID: 8878
		public static ReservationLayerDef Ceiling;

		// Token: 0x06003664 RID: 13924 RVA: 0x001D1191 File Offset: 0x001CF591
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}
	}
}
