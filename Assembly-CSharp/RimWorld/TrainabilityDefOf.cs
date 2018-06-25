using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095C RID: 2396
	[DefOf]
	public static class TrainabilityDefOf
	{
		// Token: 0x040022A9 RID: 8873
		public static TrainabilityDef None;

		// Token: 0x040022AA RID: 8874
		public static TrainabilityDef Simple;

		// Token: 0x040022AB RID: 8875
		public static TrainabilityDef Intermediate;

		// Token: 0x040022AC RID: 8876
		public static TrainabilityDef Advanced;

		// Token: 0x06003666 RID: 13926 RVA: 0x001D0EE1 File Offset: 0x001CF2E1
		static TrainabilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainabilityDefOf));
		}
	}
}
