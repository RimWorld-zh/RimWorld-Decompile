using System;

namespace RimWorld
{
	// Token: 0x02000927 RID: 2343
	[DefOf]
	public static class TransferableSorterDefOf
	{
		// Token: 0x04001FE8 RID: 8168
		public static TransferableSorterDef Category;

		// Token: 0x04001FE9 RID: 8169
		public static TransferableSorterDef MarketValue;

		// Token: 0x06003631 RID: 13873 RVA: 0x001D0B27 File Offset: 0x001CEF27
		static TransferableSorterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TransferableSorterDefOf));
		}
	}
}
