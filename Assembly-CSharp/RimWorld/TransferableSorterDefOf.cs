using System;

namespace RimWorld
{
	// Token: 0x02000925 RID: 2341
	[DefOf]
	public static class TransferableSorterDefOf
	{
		// Token: 0x04001FE8 RID: 8168
		public static TransferableSorterDef Category;

		// Token: 0x04001FE9 RID: 8169
		public static TransferableSorterDef MarketValue;

		// Token: 0x0600362D RID: 13869 RVA: 0x001D09E7 File Offset: 0x001CEDE7
		static TransferableSorterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TransferableSorterDefOf));
		}
	}
}
