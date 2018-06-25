using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095C RID: 2396
	[DefOf]
	public static class TrainabilityDefOf
	{
		// Token: 0x040022B0 RID: 8880
		public static TrainabilityDef None;

		// Token: 0x040022B1 RID: 8881
		public static TrainabilityDef Simple;

		// Token: 0x040022B2 RID: 8882
		public static TrainabilityDef Intermediate;

		// Token: 0x040022B3 RID: 8883
		public static TrainabilityDef Advanced;

		// Token: 0x06003666 RID: 13926 RVA: 0x001D11B5 File Offset: 0x001CF5B5
		static TrainabilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainabilityDefOf));
		}
	}
}
