using System;

namespace RimWorld
{
	// Token: 0x02000957 RID: 2391
	[DefOf]
	public static class BillStoreModeDefOf
	{
		// Token: 0x0600365F RID: 13919 RVA: 0x001D0D6B File Offset: 0x001CF16B
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}

		// Token: 0x040022A2 RID: 8866
		public static BillStoreModeDef DropOnFloor;

		// Token: 0x040022A3 RID: 8867
		public static BillStoreModeDef BestStockpile;

		// Token: 0x040022A4 RID: 8868
		public static BillStoreModeDef SpecificStockpile;
	}
}
