using System;

namespace RimWorld
{
	// Token: 0x02000930 RID: 2352
	[DefOf]
	public static class TrainableDefOf
	{
		// Token: 0x06003638 RID: 13880 RVA: 0x001D0AAD File Offset: 0x001CEEAD
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}

		// Token: 0x04002045 RID: 8261
		public static TrainableDef Tameness;

		// Token: 0x04002046 RID: 8262
		public static TrainableDef Obedience;

		// Token: 0x04002047 RID: 8263
		public static TrainableDef Release;
	}
}
