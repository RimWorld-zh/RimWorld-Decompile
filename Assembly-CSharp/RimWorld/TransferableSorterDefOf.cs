using System;

namespace RimWorld
{
	// Token: 0x02000927 RID: 2343
	[DefOf]
	public static class TransferableSorterDefOf
	{
		// Token: 0x04001FEF RID: 8175
		public static TransferableSorterDef Category;

		// Token: 0x04001FF0 RID: 8176
		public static TransferableSorterDef MarketValue;

		// Token: 0x06003631 RID: 13873 RVA: 0x001D0DFB File Offset: 0x001CF1FB
		static TransferableSorterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TransferableSorterDefOf));
		}
	}
}
