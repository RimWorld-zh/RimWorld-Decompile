using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095C RID: 2396
	[DefOf]
	public static class ReservationLayerDefOf
	{
		// Token: 0x06003667 RID: 13927 RVA: 0x001D0B95 File Offset: 0x001CEF95
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}

		// Token: 0x040022A7 RID: 8871
		public static ReservationLayerDef Floor;

		// Token: 0x040022A8 RID: 8872
		public static ReservationLayerDef Ceiling;
	}
}
