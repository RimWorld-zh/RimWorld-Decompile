using System;

namespace RimWorld
{
	// Token: 0x0200095B RID: 2395
	[DefOf]
	public static class BillStoreModeDefOf
	{
		// Token: 0x06003666 RID: 13926 RVA: 0x001D0B83 File Offset: 0x001CEF83
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}

		// Token: 0x040022A4 RID: 8868
		public static BillStoreModeDef DropOnFloor;

		// Token: 0x040022A5 RID: 8869
		public static BillStoreModeDef BestStockpile;

		// Token: 0x040022A6 RID: 8870
		public static BillStoreModeDef SpecificStockpile;
	}
}
