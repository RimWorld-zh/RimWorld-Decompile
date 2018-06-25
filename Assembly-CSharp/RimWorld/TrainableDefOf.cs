using System;

namespace RimWorld
{
	// Token: 0x02000932 RID: 2354
	[DefOf]
	public static class TrainableDefOf
	{
		// Token: 0x04002045 RID: 8261
		public static TrainableDef Tameness;

		// Token: 0x04002046 RID: 8262
		public static TrainableDef Obedience;

		// Token: 0x04002047 RID: 8263
		public static TrainableDef Release;

		// Token: 0x0600363C RID: 13884 RVA: 0x001D0BED File Offset: 0x001CEFED
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}
	}
}
