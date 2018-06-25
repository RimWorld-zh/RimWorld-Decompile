using System;

namespace RimWorld
{
	// Token: 0x02000959 RID: 2393
	[DefOf]
	public static class BillStoreModeDefOf
	{
		// Token: 0x040022A3 RID: 8867
		public static BillStoreModeDef DropOnFloor;

		// Token: 0x040022A4 RID: 8868
		public static BillStoreModeDef BestStockpile;

		// Token: 0x040022A5 RID: 8869
		public static BillStoreModeDef SpecificStockpile;

		// Token: 0x06003663 RID: 13923 RVA: 0x001D0EAB File Offset: 0x001CF2AB
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}
	}
}
