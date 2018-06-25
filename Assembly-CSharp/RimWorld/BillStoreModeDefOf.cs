using System;

namespace RimWorld
{
	// Token: 0x02000959 RID: 2393
	[DefOf]
	public static class BillStoreModeDefOf
	{
		// Token: 0x040022AA RID: 8874
		public static BillStoreModeDef DropOnFloor;

		// Token: 0x040022AB RID: 8875
		public static BillStoreModeDef BestStockpile;

		// Token: 0x040022AC RID: 8876
		public static BillStoreModeDef SpecificStockpile;

		// Token: 0x06003663 RID: 13923 RVA: 0x001D117F File Offset: 0x001CF57F
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}
	}
}
